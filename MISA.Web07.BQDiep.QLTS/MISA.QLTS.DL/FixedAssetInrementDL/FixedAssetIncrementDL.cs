using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class FixedAssetIncrementDL : BaseDL<FixedAssetIncrement>, IFixedAssetIncrementDL
    {
        /// <summary>
        /// Lấy danh sách chi tiết 1 bản ghi 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public FixedAssetIncrement GetAssetIncrementDetail(Guid id)
        {

            var paramater = new DynamicParameters();
            paramater.Add("v_ID", id);
            var sqlCommand = "Proc_Fixed_Asset_Increment_Detail";
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.QueryFirstOrDefault<FixedAssetIncrement>(sql: sqlCommand, paramater, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }    

        }
        #region method
        /// <summary>
        ///  Lấy danh sách tài sản theo mã ghi tăng
        /// </summary>
        /// <param name="id">id của bảng ghi tăng</param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public List<FixedAssetMulti> GetMultiAsset(Guid id,List<Guid> cacheValue, string keyword)
        {
            var recordAssets = new List<FixedAssetMulti>();
            var paramater = new DynamicParameters();
            var paramaterAsset = new DynamicParameters();
            var conditionWhere = string.Empty;
            var andCondition = string.Empty;
            if (keyword != null)
            {
                andCondition = $"FixedAssetCode LIKE '%{keyword}%'";
            }
            if (id != Guid.Empty)
            {
                 conditionWhere = $"fai.fixedAssetIncrementID='{id}'";
                if (andCondition != string.Empty)
                {
                    conditionWhere =   andCondition + " and " + conditionWhere + $" or FixedAssetName LIKE '%{keyword}%' and " + conditionWhere;
                }
                paramater.Add("v_where", conditionWhere);
            }
           
            var dataFixedAssetID =string.Empty;
            var dataAsset = new List<FixedAssetMulti>();
            if(cacheValue != null)
            {
                if (cacheValue.Count > 0)
                {
                    dataFixedAssetID = "fixedAssetID IN(";
                    foreach (var item in cacheValue)
                    {
                        dataFixedAssetID = dataFixedAssetID + "'" + item + "'" + ",";
                    }
                    dataFixedAssetID = dataFixedAssetID.Remove(dataFixedAssetID.Length - 1) + ")";
                    if(keyword != null)
                    {
                        dataFixedAssetID =$" FixedAssetCode LIKE '%{keyword}%'" + "and " + dataFixedAssetID  + $" or FixedAssetName LIKE '%{keyword}%'" + " and " + dataFixedAssetID;
                    }  
                    
                        paramaterAsset.Add("v_where", dataFixedAssetID);
                   
                }
            }
           
          
            var nameProcedure = "Proc_fixed_asset_GetDetail_Multi";
            var procName = "Proc_Asset_Multi";
            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                if(id != Guid.Empty)
                {
                    var res = sqlConnection.Query<FixedAssetMulti>(nameProcedure, paramater, commandType: System.Data.CommandType.StoredProcedure);
                    foreach (var item in res)
                    {
                        dataAsset.Add(item);
                    }
                    dataAsset = (List<FixedAssetMulti>)res;
                }
                if (dataFixedAssetID != "")
                {
                    var dataAssetCache = sqlConnection.Query<FixedAssetMulti>(procName, paramaterAsset, commandType: System.Data.CommandType.StoredProcedure);
                        foreach (var item in dataAssetCache)
                            {
                              dataAsset.Add(item);
                            }    
                 
                }
                return dataAsset;
            }
        }

        /// <summary>
        /// Cập nhật lại nguồn hình thành
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
           /// Created:Bùi Quang Điệp(09/10/2022)
        public bool UpdateSourceCost(UpdateSourceCost data)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@fixedAssetID", data.FixedAssetID);
            parameter.Add("@sumCost", data.Sumcost);
            parameter.Add("@price", data.DataSource);
            //var sqlCommand1 = $"UPDATE fixed_asset_incrment SET Price = '{data.DataSource}' WHERE fixedAssetIncrementID ='{data.FixedAssetIncrementID}' ;";
            var sqlCommand = $"UPDATE fixed_asset SET Price = '{data.DataSource}' , cost = {data.Sumcost} WHERE fixedAssetID ='{data.FixedAssetID}' ;";
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
               // sqlConnection.Execute(sqlCommand1);
                var res = sqlConnection.Execute(sqlCommand, parameter);
                if(res >0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy : Bùi Quang Điệp(12/10/2022)
        public bool CheckDoubleKeyCode(FixedAssetIncrement increment, Guid id)
        {

            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // 3 Lấy dữ liệu
                var sqlCommand = string.Empty;
                if (id != Guid.Empty)
                {
                    sqlCommand = $"SELECT FixedAssetIncrementCode FROM fixed_asset_increment  WHERE FixedAssetIncrementCode = @Code AND FixedAssetIncrementID != @ID";
                }
                else
                    sqlCommand = $"SELECT FixedAssetIncrementCode FROM fixed_asset_increment  WHERE FixedAssetIncrementCode = @Code";

                var parametes = new DynamicParameters();
                parametes.Add("@Code", increment.FixedAssetIncrementCode);
                parametes.Add("@ID", id);
                var res = sqlConection.QueryFirstOrDefault(sql: sqlCommand, parametes);
                if (res == null)
                    return true;
                else
                    return false;
            }

        }

        /// <summary>
        /// TÌm kiếm và phân trang bảng Increment
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy :Bùi Quang Điệp(12/10/2022)
        public PagingData<FixedAssetIncrement> FilterIncrement(string? where, int pageSize = 10, int pageNumber = 1)
        {
           
            var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString);

      
            var sqlCommand = "Proc_asset_increment_GetPaging";
            // Chuẩn bị tham số đầu vào cho stored procedure
            var parameters = new DynamicParameters();
            parameters.Add("@v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("@v_Limit", pageSize);
            parameters.Add("@v_Sort", "ModifiedDate DESC");
            parameters.Add("@v_Where", where);

            // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
            var multipleResults = sqlConection.QueryMultiple(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về từ DB
            if (multipleResults != null)
            {
                var data = multipleResults.Read<FixedAssetIncrement>().ToList();
                var totalCount = multipleResults.Read<long>().Single();
             //   var summary = multipleResults.Read<SummaryAsset>().ToList();
                decimal sumQuantity = 0;
                decimal sumPrice = 0;
                decimal sumDepreciations = 0;
                decimal sumAtrophy = 0;
                var now = DateTime.Now.Year;
                //foreach (var val in summary)
                //{
                //    sumQuantity = sumQuantity + val.quantity;
                //    sumPrice = sumPrice + val.cost;
                //    var year = val.productionDate.Year;
                //    var yearUse = now - year;
                //    if (yearUse == 0)
                //    {
                //        yearUse = 1;
                //    }
                //    var sumDepreciation = val.cost * (val.depreciationRate * yearUse / 100);
                //    sumDepreciations = sumDepreciations + sumDepreciation;
                //    var residualValue = val.cost - sumDepreciation;
                //    sumAtrophy = sumAtrophy + residualValue;
                //}
                //var dataSummary = new List<decimal>();
                //dataSummary.Add(sumQuantity);
                //dataSummary.Add(sumPrice);
                //dataSummary.Add(sumDepreciations);
                //dataSummary.Add(sumAtrophy);
                var res = new PagingData<FixedAssetIncrement>()
                {
                    Data = data,
                    TotalCount = totalCount,
                  //  Summary = dataSummary
                };
                return res;
            }
            else
            {
                return new PagingData<FixedAssetIncrement>();
            }
        }
        /// <summary>
        /// Lấy tên chứng từ ghi tăng theo mã tài sản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreateBy :Bùi Quang Điệp (19/10/2022)
        public string GetAssetIncrementName(Guid id)
        {
            if (id != Guid.Empty)
            {
                var nameProcedure = "Proc_FixedAsset_Increment_GetName";
                var param = new DynamicParameters();
                param.Add("id", id);
                using(var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    var value = sqlConection.QueryFirstOrDefault<string>(nameProcedure, param, commandType:System.Data.CommandType.StoredProcedure);
                    return value.ToString();

                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
