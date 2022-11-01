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
            var procName = ResourceProcedure.GetAllRecord;
            var pram = new DynamicParameters();
            pram.Add("v_TableName", name);
            //  Lấy dữ liệu
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConection.Query(procName, pram, commandType: System.Data.CommandType.StoredProcedure);

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
            var nameTable = GetTableName();
            // truy vấn dữ liệu
            var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(KeyAttribute));
            // lấy tên key của bảng
            var nameKey = (attribute as KeyAttribute).Key;

            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConection.Open();

                using (var trans = sqlConection.BeginTransaction())
                {
                    var conditionWhere = " IN(";
                    for (int i = 0; i < ids.Count; i++)
                    {
                        conditionWhere = conditionWhere + $"'{ids[i]}',";
                        if (typeof(T).Name == "FixedAssetIncrement")
                        {
                            HandlerMajorDeleteIncrement(ids[i], sqlConection, trans);

                        }
                    }
                    conditionWhere = conditionWhere.Remove(conditionWhere.Length - 1) + ")";
                    var parameter = new DynamicParameters();
                    parameter.Add("v_nameTable", nameTable);
                    parameter.Add("v_nameCode", nameKey);
                    parameter.Add("v_where", conditionWhere);
                    var proc = ResourceProcedure.DeleteRecord;
                    var numberRows = sqlConection.QueryFirstOrDefault<int>(proc, parameter, transaction: trans, commandType: System.Data.CommandType.StoredProcedure);
                    trans.Commit();
                    return numberRows;
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
                            HandlerMajorDeleteIncrement(id, sqlConnection, trans);
                            UpdateBudget(proppertiesAssetIDs, propertiesAssets, sqlConnection, trans, result);
                        }
                        trans.Commit();
                    }
                    return result;
                }
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

            paramater.Add("v_KeyWord", search);
            paramater.Add("v_NameTable", nameTable);
            var procSearch = ResourceProcedure.SearchRecord;

            // Kết nối đến database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var res = sqlConnection.Query(procSearch, paramater, commandType: System.Data.CommandType.StoredProcedure);
                return res;
            }
        }

        /// <summary>
        /// Lấy mã tài sản mới nhất
        /// <returns>mã tài sản  mới nhất</returns>
        /// Created by: Bùi Quang Điệp (07/10/2022)
        public List<string> GetNewCode()
        {
            var recordCodes = new List<string>();
            // Lấy tên của table
            var nameTable = GetTableName();
            // Lấy trường Code 
            var codeTable = GetCodeTable();
            // 3 Lấy dữ liệu
            var proc = ResourceProcedure.GetNewCode;
            var param = new DynamicParameters();
            param.Add("nameTable", nameTable);
            param.Add("codeTable", codeTable);
            using (var sqlConection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var multipleResults = sqlConection.QueryMultiple(proc, param, commandType: System.Data.CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    recordCodes.Add(multipleResults.Read<string>().Single());
                    recordCodes.Add(multipleResults.Read<string>().Single());
                }
                return recordCodes;
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
        protected virtual void HandlerMajorDeleteIncrement(Guid fixedAssetIncrementID, MySqlConnection sqlConection, MySqlTransaction trans){}

        /// <summary>
        /// Xử lý nghiệp vụ ghi tăng tài sản
        /// </summary>
        /// <param name="proppertiesAssetIDs"></param>
        /// <param name="propertiesAssets"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        /// <param name="result"></param>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        protected virtual void UpdateBudget(List<Guid>? proppertiesAssetIDs, List<UpdateSourceCost>? propertiesAssets, MySqlConnection sqlConnection, MySqlTransaction trans, Guid result) { }
        #endregion
    }
}
