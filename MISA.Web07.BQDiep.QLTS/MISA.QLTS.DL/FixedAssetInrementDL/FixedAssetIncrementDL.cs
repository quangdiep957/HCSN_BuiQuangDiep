using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
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
            var sqlCommand = ResourceProcedure.GetAssetIncrementDetail;
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
        public List<FixedAssetMulti> GetMultiAsset(Guid id, string keyword)
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
                    conditionWhere = andCondition + " and " + conditionWhere + $" or FixedAssetName LIKE '%{keyword}%' and " + conditionWhere;
                }
                paramater.Add("v_where", conditionWhere);
            }
            var dataAsset = new List<FixedAssetMulti>();
            var nameProcedure = ResourceProcedure.GetAssetByIncrement;
            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                if (id != Guid.Empty)
                {
                    var res = sqlConnection.Query<FixedAssetMulti>(nameProcedure, paramater, commandType: System.Data.CommandType.StoredProcedure);
                    foreach (var item in res)
                    {
                        dataAsset.Add(item);
                    }
                    dataAsset = (List<FixedAssetMulti>)res;
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
        public bool CheckDuplicationCode(FixedAssetIncrement increment, Guid id)
        {
            var procName = string.Empty;
            var parametes = new DynamicParameters();
            if (id != Guid.Empty)
            {
                procName = ResourceProcedure.CheckDuplicationIncrementCodeUpdate;
            }
            else
            {
                procName = ResourceProcedure.CheckDuplicationIncrementCodeCreate;
            }
            parametes.Add("v_FixedAssetIncrementCode", increment.FixedAssetIncrementCode);
            parametes.Add("v_FixedAssetIncrementID", id);
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.QueryFirstOrDefault(procName, parametes, commandType: System.Data.CommandType.StoredProcedure);
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
            var sqlCommand = ResourceProcedure.GetAssetIncrementPaging;
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
                var summary = multipleResults.Read<Summary>().Single();
                var now = DateTime.Now.Year;
                var dataSummary = new Summary();
                dataSummary = summary;
                var res = new PagingData<FixedAssetIncrement>()
                {
                    Data = data,
                    TotalCount = totalCount,
                    Summary = dataSummary
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
                var nameProcedure = ResourceProcedure.GetAssetIncrementName;
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

        /// <summary>
        /// Xử lý nghiệp vụ ghi tăng tài sản
        /// </summary>
        /// <param name="proppertiesAssetIDs"></param>
        /// <param name="propertiesAssets"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        /// <param name="result"></param>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        protected override void MajorUpdateRecord(List<Guid>? proppertiesAssetIDs, List<UpdateSourceCost>? propertiesAssets, MySqlConnection sqlConnection, MySqlTransaction trans, Guid result)
        {
            var paramaterCreate = new DynamicParameters();
            var paramaterUpdateAsset = new DynamicParameters();
            // Thêm mới vào bảng chi tiết 
            var valueInsert = " ";
            var valueUpdateAsset = "(";
            // lấy danh sách Asset ID 
            for (int i = 0; i < proppertiesAssetIDs.Count; i++)
            {
                if (i < proppertiesAssetIDs.Count - 1)
                {
                    valueInsert = valueInsert + $"('{result}','{proppertiesAssetIDs[i]}')" + ",";
                    valueUpdateAsset = valueUpdateAsset + "'" + proppertiesAssetIDs[i] + "'" + ",";
                }
                else
                {
                    valueInsert = valueInsert + $"('{result}','{proppertiesAssetIDs[i]}')";
                    valueUpdateAsset = valueUpdateAsset + "'" + proppertiesAssetIDs[i] + "'" + ")";
                }
            }

            // Cập nhật nguồn hình thành
            var updateAsset = "";
            var sumCosts = " fa.Cost = (case ";
            var dataSource = " fa.Price = (case ";
            var fixedAssetIDs = " WHERE FixedAssetID in (";
            // lấy danh sách nguồn hình thành
            for (int i = 0; i <= propertiesAssets.Count; i++)
            {
                if (i < propertiesAssets.Count)
                {
                    sumCosts = sumCosts + $"when fa.FixedAssetID = '{propertiesAssets[i].FixedAssetID}' then '{propertiesAssets[i].Sumcost}'";
                    fixedAssetIDs = fixedAssetIDs + $"'{propertiesAssets[i].FixedAssetID}',";
                    dataSource = dataSource + $"when fa.FixedAssetID = '{propertiesAssets[i].FixedAssetID}' then '{propertiesAssets[i].DataSource}'";
                }
                else
                {
                    sumCosts = sumCosts + "end),";
                    fixedAssetIDs = fixedAssetIDs.Remove(fixedAssetIDs.Length - 1) + ")";
                    dataSource = dataSource + "end)";
                }

            }
            var procCreateDetail = ResourceProcedure.CreateAssetIncrementDetail;
            paramaterCreate.Add("v_where", valueInsert);
            sqlConnection.Execute(procCreateDetail, paramaterCreate, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            // Cập nhật trạng thái
            paramaterUpdateAsset.Add("v_where", valueUpdateAsset);
            paramaterUpdateAsset.Add("v_status", Status.Active);
            var procUpdateStatus = ResourceProcedure.UpdateStatus;
            sqlConnection.Execute(procUpdateStatus, paramaterUpdateAsset, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);

            // Kiểm tra nguồn hình thành có dữ liệu hay không
            if (propertiesAssets.Count > 0)
            {
                updateAsset = updateAsset + sumCosts + dataSource + fixedAssetIDs;
                var pocUpdateBudget = ResourceProcedure.UpdateBudget;
                var pramUpdateBudget = new DynamicParameters();
                pramUpdateBudget.Add("v_Set", updateAsset);
                sqlConnection.Execute(pocUpdateBudget, pramUpdateBudget, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            }
        }

        /// <summary>
        /// Xử lý các nghiệp vụ xóa của ghi tăng tài sản
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="nameTable"></param>
        /// <param name="valueKey"></param>
        /// <param name="sqlConection"></param>
        /// <param name="trans"></param>
        /// <param name="parameterDeleteDetail"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        /// createBy : Bùi Quang Điệp (24/10/2022)
        protected override void HandlerMajorDelete(Guid fixedAssetIncrementID, MySqlConnection sqlConection, MySqlTransaction trans)
        {
            // Lấy danh sách ID tài sản theo mã ghi tăng
            var proc = ResourceProcedure.GetAssetByIncrement;
            var updateStatus = string.Empty;
            var param = new DynamicParameters();
            var dataFixedAssetID = $"fai.fixedAssetIncrementID='{fixedAssetIncrementID}'";
            param.Add("v_where", dataFixedAssetID);
            var fixedAssetIDs = sqlConection.Query<FixedAssetMulti>(proc, param, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            // Cập nhật lại trạng thái của các tài sản tìm thấy theo mã ghi tăng
            updateStatus = "(";
            foreach (var id in fixedAssetIDs)
            {
                updateStatus = updateStatus + $"'{id.FixedAssetID}',";

            }
            updateStatus = updateStatus.Remove(updateStatus.Length - 1) + ")";
            // tạo câu truy vấn 
            // Cập nhật trạng thái về 0
            var paramUpdate = new DynamicParameters();
            paramUpdate.Add("v_where", updateStatus);
            paramUpdate.Add("v_status", Status.NoActive);
            var procUpdateStatus = ResourceProcedure.UpdateStatus;
            sqlConection.Execute(procUpdateStatus, paramUpdate, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            // Xóa các dữ liệu trong bảng chi tiết chứng từ
            var parameterDeleteDetail = new DynamicParameters();
            var procDeleteIncrementDetail = ResourceProcedure.DeleteIncrementDetail;
            parameterDeleteDetail.Add("v_FixedAssetIncrementID", fixedAssetIncrementID);
            sqlConection.Execute(procDeleteIncrementDetail, param: parameterDeleteDetail, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
        }
        #endregion
    }
}
