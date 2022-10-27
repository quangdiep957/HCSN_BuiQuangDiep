using Dapper;
using Microsoft.VisualBasic;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {

        #region Field
        //private const string CONNECTING = "Server=localhost;Port=3306;Database=misa.web07.hcsn.diep;Uid=root;Pwd=Quangdiep@2001;";
        #endregion
        #region method

        /// <summary>
        /// Lấy tất cả bản ghi của 1 bản
        /// </summary>
        /// <returns>Tất cả bản ghi của một bảng</returns>
        /// create By : Bùi Quang Điệp(23/08/2022)
        public virtual IEnumerable<dynamic> GetAll()
        {
            // Lấy tên của bảng tương ứng với class (nếu có thiết lập thông qua Attribut custom)
            var name = GetTableName();
            var procName = Procedure.GetAllRecord;
            var pram = new DynamicParameters();
            pram.Add("v_TableName", name);
            //  Lấy dữ liệu
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.Query(procName,pram,commandType: System.Data.CommandType.StoredProcedure);

                // 4 Trả về kết quả
                return res;
            }
        }

        /// <summary>
        /// Xóa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Xóa 1 bản ghi trong 1 bảng</returns>
        /// createBy : Bùi Quang Điệp (24/08/2022)
        public int Delete(List<Guid> ids)
        {
            var numbers = 0;
            // Lấy tên của bảng tương ứng với Class
            var name = GetTableName();

            // Lấy Id của bảng tương ứng
            // var key =(typeof(T).Name).FindPrimaryKey();
            // truy vấn dữ liệu
            var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(KeyAttribute));
            var vlkey = (attribute as KeyAttribute).Key;

            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConection.Open();

                using (var trans = sqlConection.BeginTransaction())
                {
                    var parameters = new DynamicParameters();
                    for (int i = 0; i < ids.Count; i++)
                    {
                        var sqlCommands = string.Empty;
                        if (typeof(T).Name == "FixedAssetIncrement")
                        {
                            // Lấy danh sách ID tài sản
                            var proc = Procedure.GetAssetByIncrement;
                            var queryDelete = string.Empty;
                            var param = new DynamicParameters();
                            var dataFixedAssetID = $"fai.fixedAssetIncrementID='{ids[i]}'";
                            param.Add("v_where", dataFixedAssetID);
                            queryDelete = "Update fixed_asset set status = 0 where fixedAssetID IN(";
                            var fixedAssetIDs = sqlConection.Query<FixedAssetMulti>(proc, param, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                            foreach (var id in fixedAssetIDs)
                            {
                                queryDelete = queryDelete + $"'{id.FixedAssetID}',";

                            }
                            queryDelete = queryDelete.Remove(queryDelete.Length - 1) + ")";
                            // tạo câu truy vấn 
                            sqlConection.Execute(queryDelete, transaction: trans);

                            sqlCommands = $"Delete From {name}_detail where {vlkey} = @Delete";


                            parameters.Add("@Delete", ids[i]);
                            var numberRowsDetail = sqlConection.Execute(sqlCommands, param: parameters, transaction: trans);

                        }

                        var parameter = new DynamicParameters();
                        parameter.Add("@Delete", ids[i]);
                        var sqlCommand = $"Delete From {name} where {vlkey} = @Delete";
                        var numberRows = sqlConection.Execute(sqlCommand, param: parameter, transaction: trans);
                        numbers = numbers + numberRows;


                    }

                    trans.Commit();
                    return numbers;
                }

            }


        }

        /// <summary>
        /// Lấy tên của table trong database nếu có định nghĩa theo Attribute Custom
        /// </summary>
        /// <returns>Tên bảng</returns>
        /// CreatedBy: Bùi Quang Điệp (24/08/2022)
        private string GetTableName()
        {
            var isDefineAttributeTableName = Attribute.IsDefined(typeof(T), typeof(TableName));
            if (isDefineAttributeTableName)
            {
                var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(TableName));
                // Lấy ra tên:
                var name = (attribute as TableName).Name; // ép về kiểu Attribute TableName mà mình tự định nghĩa;
                return name;

            }
            else
            {
                // Nếu không khai báo table name thì trả về tên Class 
                return typeof(T).Name;
            }
        }

        /// <summary>
        /// Lấy tên của table trong database nếu có định nghĩa theo Attribute Custom
        /// </summary>
        /// <returns>Tên bảng</returns>
        /// CreatedBy: Bùi Quang Điệp (24/08/2022)
        private string GetCodeTable()
        {
            var isDefineAttributeTableName = Attribute.IsDefined(typeof(T), typeof(CodeAtrribute));
            if (isDefineAttributeTableName)
            {
                var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(CodeAtrribute));
                // Lấy ra tên:
                var name = (attribute as CodeAtrribute).Name; // ép về kiểu Attribute mà mình tự định nghĩa;
                return name;

            }
            else
            {
                // Nếu không khai báo table name thì trả về tên Class 
                return typeof(T).Name;
            }
        }

        /// <summary>
        /// Thêm mới 1 bản ghi vào 1 Bảng
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Giá trị cảu bản ghi được thêm mới</returns>
        ///  /// createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid InsertOneRecord(T record)
        {
            // Lấy tên của table
            var nameTable = GetTableName();
            var proppertiesAssetIDs = new List<Guid>();
            var propertiesAssets = new List<UpdateSourceCost>();
            // Lấy tên của Procedure insert 1 bảng ghi của table đó
            var nameProcedure = $"Proc_{nameTable}_OneInsert";

            // Lấy tham số đầu vào
            var properties = typeof(T).GetProperties();
            var paramater = new DynamicParameters();
            foreach (var propertie in properties)
            {
                var propertyName = $"{propertie.Name}";
                var propertyValue = propertie.GetValue(record);
                if (propertyName != "FixedAssetIDs")
                {
                    if (propertyName != "FixedAssets")
                    {
                        paramater.Add(propertyName, propertyValue);
                    }
                    else
                    {
                        propertiesAssets = (List<UpdateSourceCost>?)propertie.GetValue(record);
                    }
                }
                else
                {
                    proppertiesAssetIDs = (List<Guid>?)propertie.GetValue(record);
                }
            }

            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction())
                {

                    var numberAffectedRow = sqlConnection.Execute(nameProcedure, paramater, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                    var result = Guid.Empty;
                    if (numberAffectedRow > 0)
                    {
                        result = (Guid)properties[0].GetValue(record);

                        // Kiểm tra xem có phải bảng ghi tăng hay không
                        if (typeof(T).Name == "FixedAssetIncrement")
                        {
                            UpdateBudget(proppertiesAssetIDs, propertiesAssets, sqlConnection, trans, result);
                        }
                        trans.Commit();
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Sửa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <param name=""></param>
        /// <param name="id"></param>
        /// <returns>Trả về mã bản ghi đã được sửa</returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid UpdateRecord(T record, Guid id)
        {
            var nameTable = GetTableName();
            var proppertiesAssetIDs = new List<Guid>();
            var propertiesAssets = new List<UpdateSourceCost>();
            var nameProcedure = $"Proc_{nameTable}_Update";
            var parameter = new DynamicParameters();
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties)
            {
                var propertyName = $"v_{propertie.Name}";
                var propertyValue = propertie.GetValue(record);
                if (String.Compare(propertyName, $"v_{typeof(T).Name}ID", true) == 0)
                {
                    parameter.Add(propertyName, id);
                }
                else if (propertyName != "v_FixedAssetIDs")
                {
                    if (propertyName != "v_FixedAssets")
                    {
                        parameter.Add(propertyName, propertyValue);
                    }
                    else
                    {
                        propertiesAssets = (List<UpdateSourceCost>?)propertie.GetValue(record);
                    }
                }
                else
                {
                    proppertiesAssetIDs = (List<Guid>?)propertie.GetValue(record);
                }
            }

            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction())
                {
                    var numberAffectedRow = sqlConnection.Execute(nameProcedure, parameter, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                    var result = Guid.Empty;
                    if (numberAffectedRow > 0)
                    {
                        result = id;
                        // Kiểm tra xem có phải bảng ghi tăng hay không
                        if (typeof(T).Name == "FixedAssetIncrement")
                        {
                            RemoveBudgetOld(id, sqlConnection, trans);
                            UpdateBudget(proppertiesAssetIDs, propertiesAssets, sqlConnection, trans, result);
                        }
                        trans.Commit();
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Cập nhật lại tất cả trạng thái của tài sản về chưa sử dụng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        private static void RemoveBudgetOld(Guid id, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            // Lấy danh sách ID tài sản
            var nameProcAssetID = "Proc_fixed_asset_GetDetail_Multi";
            var conditionWhere = $"fai.fixedAssetIncrementID='{id}'";
            var paramAssetID = new DynamicParameters();
            paramAssetID.Add("v_where", conditionWhere);
            var queryDelete = "Update fixed_asset set status = 0 where fixedAssetID IN(";
            var AssetIDs = sqlConnection.Query<FixedAsset>(nameProcAssetID, paramAssetID, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            foreach (var assetID in AssetIDs)
            {
                queryDelete = queryDelete + $"'{assetID.FixedAssetID}',";
            }
            queryDelete = queryDelete.Remove(queryDelete.Length - 1) + ")";
            // tạo câu truy vấn 
            sqlConnection.Execute(queryDelete, transaction: trans);
            var removeDetail = $"delete from fixed_asset_increment_detail where FixedAssetIncrementID = '{id}'";
            var numberAffectedDetail = sqlConnection.Execute(removeDetail, transaction: trans);
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
        private static void UpdateBudget(List<Guid>? proppertiesAssetIDs, List<UpdateSourceCost>? propertiesAssets, MySqlConnection sqlConnection, MySqlTransaction trans, Guid result)
        {
            var paramaters = new DynamicParameters();
            var paramaterUpdateAsset = new DynamicParameters();
            var valueInsert = " INSERT INTO fixed_asset_increment_detail VALUES";
            var valueUpdateAsset = "UPDATE fixed_asset SET Status=1 WHERE FixedAssetID IN(";
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
            var updateAsset = "UPDATE fixed_asset fa ";
            var sumCosts = " SET fa.Cost = (case ";
            var dataSource = " fa.Price = (case ";
            var fixedAssetIDs = " WHERE FixedAssetID in (";
            // lấy danh sách Asset ID 
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
            paramaters.Add("valueInsert", valueInsert);
            paramaterUpdateAsset.Add("value", valueUpdateAsset);
            var numberAffectedRows = sqlConnection.Execute(valueInsert, transaction: trans);
            var numberAffectedRowsUpdate = sqlConnection.Execute(valueUpdateAsset, transaction: trans);
            if (propertiesAssets.Count > 0)
            {
                updateAsset = updateAsset + sumCosts + dataSource + fixedAssetIDs;
                var numberAffectedRowsUpdateCost = sqlConnection.Execute(updateAsset, transaction: trans);
            }
        }

        /// <summary>
        /// Tìm kiếm theo mã và tên của từng table
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns> Danh sách bản ghi</returns>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        public IEnumerable<dynamic> SearchRecord(string keyword)
        {  // Lấy tên của table
            var nameTable = GetTableName();

            // Lấy tham số đầu vào
            var search = "";
            if (keyword != null)
            {
                // Kiểm tra có kí tự đặc biệt hay không 

                // Lấy tham số đầu vào
                var properties = typeof(T).GetProperties();
                var tableName = typeof(T).Name;
                var propertyName = string.Empty;
                var propertyCode = string.Empty;
                foreach (var property in properties)
                {
                    if (String.Compare(property.Name, tableName + "Code", true) == 0)
                    {
                        propertyName = property.Name;
                    }
                    if (String.Compare(property.Name, tableName + "Name", true) == 0)
                    {
                        propertyCode = property.Name;
                    }
                }

                search = $"{propertyCode} LIKE '%{keyword}%'  or {propertyName} LIKE '%{keyword}%' ";

            }
            var paramater = new DynamicParameters();

            paramater.Add("@Key", search);

            var sqlCommand = $"select * from {nameTable} where {search}";

            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConnection.Query(sqlCommand, paramater);

                return res;
            }
        }

        /// <summary>
        /// Lấy mã tài sản mới nhất
        /// <returns>mã tài sản  mới nhất</returns>
        /// Created by: Bùi Quang Điệp (07/10/2022)
        public List<string> GetNewAsset()
        {
            var recordCodes = new List<string>();
            var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString);
            // Lấy tên của table
            var nameTable = GetTableName();
            // Lấy trường Code 
            var codeTable = GetCodeTable();
            // 3 Lấy dữ liệu
            var sqlCommand = $"SELECT {codeTable} FROM {nameTable} ORDER BY {codeTable} DESC LIMIT 1";
            var assetCode = sqlConection.QueryFirstOrDefault<string>(sql: sqlCommand);
            recordCodes.Add(assetCode);
            var sqlCommand1 = $"SELECT {codeTable} FROM {nameTable} ORDER BY ModifiedDate DESC limit 1";
            var assetCode1 = sqlConection.QueryFirstOrDefault<string>(sql: sqlCommand1);


            recordCodes.Add(assetCode1);
            return recordCodes;
        }
        #endregion
    }
}
