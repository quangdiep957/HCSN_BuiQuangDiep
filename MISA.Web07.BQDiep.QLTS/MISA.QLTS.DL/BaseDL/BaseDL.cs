using Dapper;
using MISA.QLTS.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                var key = typeof(T).GetCustomAttributes<KeyAttribute>();
                var sqlCommand = $"delete from {name} where {key} = @ID";
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

    }
}
