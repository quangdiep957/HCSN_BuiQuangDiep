using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.QLTS.DL
{
    public class AssetDL : BaseDL<FixedAsset>, IAssetDL
    {

        public PagingData<FixedAsset> FilterAsset(string? where, int pageSize = 10, int pageNumber = 1)
        {
            var sqlCommand = ResourceProcedure.GetAssetPaging;
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
                    var assets = multipleResults.Read<FixedAsset>().ToList();
                    long totalCount;
                    Summary dataSummary;
                    // Gọi hàm tính tổng và lấy tổng số bản ghi
                    SummaryCount(multipleResults, out totalCount, out dataSummary);
                    var res = new PagingData<FixedAsset>()
                    {
                        Data = assets,
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
        /// Tính tổng giá tiền và tổng số trang
        /// </summary>
        /// <param name="multipleResults"></param>
        /// <param name="totalCount"></param>
        /// <param name="dataSummary"></param>
        /// createBy : Bùi Quang Điệp (24/10/2022)
        private static void SummaryCount(SqlMapper.GridReader multipleResults, out long totalCount, out Summary dataSummary)
        {
            totalCount = multipleResults.Read<long>().Single();
            var summary = multipleResults.Read<SummaryAsset>().ToList();
            decimal sumQuantity = 0;
            decimal sumPrice = 0;
            decimal sumDepreciations = 0;
            decimal sumAtrophy = 0;
            const int hundredPercent = 100;
            var now = DateTime.Now.Year;
            const int fixedDecimal = 0;
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
            dataSummary = new Summary();
            dataSummary.SumAtrophy = Math.Round(sumAtrophy, fixedDecimal);
            dataSummary.SumQuantity= Math.Round(sumQuantity, fixedDecimal);
            dataSummary.SumPrice = Math.Round(sumPrice, fixedDecimal) ;
            dataSummary.SumDepreciation = Math.Round(sumDepreciations, fixedDecimal) ;
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
            var sqlCommand = ResourceProcedure.GetAssetDetail;
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
                procName = ResourceProcedure.CheckDuplicateCodeAssetUpdate;
            }
            else
            {
                procName = ResourceProcedure.CheckDuplicateCodeAssetCreate;
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
