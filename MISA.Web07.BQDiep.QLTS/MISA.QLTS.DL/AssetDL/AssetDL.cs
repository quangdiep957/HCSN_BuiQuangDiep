using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.QLTS.DL
{
    public class AssetDL : BaseDL<FixedAsset>, IAssetDL
    {

        public PagingData<FixedAsset> FilterAsset(string? where, int pageSize = 10, int pageNumber = 1)
        {
            var sqlCommand = "Proc_asset_GetPaging";
            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_Limit", pageSize);
            parameters.Add("@v_Sort", "ModifiedDate DESC");
            parameters.Add("@v_Where", where);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var multipleResults = sqlConection.QueryMultiple(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);
                // Xử lý kết quả trả về từ DB
                if (multipleResults != null)
                {
                    var employees = multipleResults.Read<FixedAsset>().ToList();
                    var totalCount = multipleResults.Read<long>().Single();
                    var summary = multipleResults.Read<SummaryAsset>().ToList();
                    decimal sumQuantity = 0;
                    decimal sumPrice = 0;
                    decimal sumDepreciations = 0;
                    decimal sumAtrophy = 0;
                    const int hundredPercent = 100;
                    var now = DateTime.Now.Year;
                    foreach (var val in summary)
                    {
                        sumQuantity = sumQuantity + val.Quantity;
                        sumPrice = sumPrice + val.Cost;
                        var year = val.ProductionDate.Year;
                        var yearUse = now - year;
                        if (yearUse == 0)
                        {
                            yearUse = 1;
                        }
                        var sumDepreciation = val.Cost * (val.DepreciationRate * yearUse / hundredPercent);
                        sumDepreciations = sumDepreciations + sumDepreciation;
                        var residualValue = val.Cost - sumDepreciation;
                        sumAtrophy = sumAtrophy + residualValue;
                    }
                    var dataSummary = new List<decimal>();
                    dataSummary.Add(sumQuantity);
                    dataSummary.Add(sumPrice);
                    dataSummary.Add(sumDepreciations);
                    dataSummary.Add(sumAtrophy);
                    var res = new PagingData<FixedAsset>()
                    {
                        Data = employees,
                        TotalCount = totalCount,
                        Summary = dataSummary
                    };
                    return res;
                }
                else
                {
                    return new PagingData<FixedAsset>();
                }
            }
        }

        /// <summary>
        /// Lấy thông tin một bản ghi theo mã
        /// </summary>
        /// <param name="id">mã id muốn lấy</param>
        /// <returns>
        /// -200 Kết quả trả về một bản ghi 
        /// -500 Lỗi phía serve
        /// </returns>
        /// 
        public FixedAsset GetAssetDetail(Guid id)
        {
            var paramater = new DynamicParameters();
            paramater.Add("@v_ID", id);
            var sqlCommand = "Proc_Fixed_Asset_Detail";
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.QueryFirstOrDefault<FixedAsset>(sql: sqlCommand, paramater, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        /// <summary>
        /// Kiểm tra xem có mã tài sản này không
        /// </summary>
        /// <returns>Mã tài sản mới nhất</returns>
        /// /// Create By : Bùi Quang Điệp (22/08/2022)
        public bool CheckDuplicateCode(FixedAsset asset, Guid id)
        {

            var procName = string.Empty;
            var parametes = new DynamicParameters();
            if (id != Guid.Empty)
            {
                procName ="Proc_FixedAsset_CheckDuplicateAssetCode_Update";
            }
            else
            {
                procName ="Proc_FixedAsset_CheckDuplicateAssetCode_Create";
            }    
            parametes.Add("v_FixedAssetCode", asset.FixedAssetCode);
            parametes.Add("v_FixedAssetID", id);
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.QueryFirstOrDefault(procName, parametes, commandType: System.Data.CommandType.StoredProcedure);
                if (res == null)
                    return true;
                else
                    return false;
            }

        }



    }
}
