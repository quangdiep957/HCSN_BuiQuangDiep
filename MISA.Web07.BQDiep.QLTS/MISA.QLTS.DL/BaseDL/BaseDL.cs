using Dapper;
using MISA.QLTS.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLTS.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Field
        private const string CONNECTING = "Server=localhost;Port=3306;Database=misa.web07.hcsn.diep;Uid=root;Pwd=Quangdiep@2001;";
        #endregion
        /// <summary>
        /// Lấy tất cả bản ghi của 1 bản
        /// </summary>
        /// <returns>Tất cả bản ghi của một bảng</returns>
        /// create By : Bùi Quang Điệp(23/08/2022)
        public virtual IEnumerable<dynamic> GetAll()
        {
           using(var sqlConection = new MySqlConnection(CONNECTING))
            {
                // Lấy tên của bảng tương ứng với class (nếu có thiết lập thông qua Attribut custom)
                var name = GetTableName();
                // 3 Lấy dữ liệu
                var sqlCommand = $"SELECT * from {name}";
                var res = sqlConection.Query(sql: sqlCommand);

                // 4 Trả về kết quả
                return res;
            }    
           
     
        }
        /// <summary>
        /// Xóa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Xóa 1 bản ghi trong 1 bảng</returns>
        /// createBy : Bùi Quang Điệp (24/08/2022)
        public int Delete(Guid id)
        {
            using(var sqlConnection = new MySqlConnection(CONNECTING))
            {
                // Lấy tên của bảng tương ứng với Class
                var name = GetTableName();

                // Lấy Id của bảng tương ứng
                // var key =(typeof(T).Name).FindPrimaryKey();
                // truy vấn dữ liệu
                var attribute = Attribute.GetCustomAttribute(typeof(T), typeof(KeyAttribute));
                var vlkey = (attribute as KeyAttribute).Key;
                var sqlCommand = $"delete from {name} where {vlkey} = @ID";
                var parameters = new DynamicParameters();
                parameters.Add("@ID", id);
                var res = sqlConnection.Execute(sql: sqlCommand, param: parameters);
                return res;
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
        /// Thêm mới 1 bản ghi vào 1 Bảng
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Giá trị cảu bản ghi được thêm mới</returns>
        ///  /// createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid InsertOneRecord(T record)
        {
            // Lấy tên của table
            var nameTable = GetTableName();
            // Lấy tên của Procedure insert 1 bảng ghi của table đó
            var nameProcedure = $"Proc_{nameTable}_OneInsert";

            // Lấy tham số đầu vào
           var properties= typeof(T).GetProperties();
            var paramater = new DynamicParameters();
            foreach (var propertie in properties)
            {
                var propertyName = $"{propertie.Name}";
                var propertyValue = propertie.GetValue(record);
                paramater.Add(propertyName, propertyValue);
            }

            // Kết nối đến database
            using (var sqlConnection =new MySqlConnection(CONNECTING))
            {
                 var numberAffectedRow = sqlConnection.Execute(nameProcedure, paramater,commandType:System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberAffectedRow > 0)
                {
                    //var checkID = $"SELECT {properties[0]} FROM {nameTable} WHERE {properties[0]} = ";
                    //var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    // var newId = primaryKeyProperty?.GetValue(record);
                    //if (newId != null)
                    //{
                    //    result = (Guid)newId;
                    //}
                    result = (Guid)properties[0].GetValue(record);
                }
                return result;
            }    
        }

        /// <summary>
        /// Sửa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <param name=""></param>
        /// <param name="id"></param>
        /// <returns>Trả về mã bản ghi đã được sửa</returns>
        public Guid UpdateOneRecord(T record,Guid id)
        {
            var nameTable = GetTableName();
            var sqlCommand = $"Proc_{nameTable}_Update";
            var parameter = new DynamicParameters();
            var properties = typeof(T).GetProperties();
            foreach (var propertie in properties)
            {
                var propertyName = $"{propertie.Name}";
                var propertyValue = propertie.GetValue(record);
                parameter.Add(propertyName, propertyValue);
            }  
            using(var sqlConnection =new MySqlConnection(CONNECTING))
            {
                var numberAffectedRow = sqlConnection.Execute(sqlCommand, parameter, commandType: System.Data.CommandType.StoredProcedure);
                var result = Guid.Empty;
                if (numberAffectedRow > 0)
                {
                   
                    result = (Guid)properties[0].GetValue(record);
                }
                return result;
            }    
        }
    }
}
