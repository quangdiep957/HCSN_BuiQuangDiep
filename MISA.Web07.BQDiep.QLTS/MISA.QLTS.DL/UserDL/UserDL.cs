using Dapper;
using MISA.QLSP.Common.Entities.Entities;
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
    public class UserDL : IUserDL
    {

        /// <summary>
        /// xử lý sự kiện đăng nhập
        /// </summary>
        /// <param name="user"></param>
        /// <returns>trả về kết quả đằng nhập thành công hay không</returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        public User Login(User user)
        {
            var param = new DynamicParameters();
            param.Add("@UserName",user.UserName);
            param.Add("@Password",user.Password);

            var sqlcommand = "Select * from User where UserName = @UserName and Password = @Password";

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var affectedRow = sqlConnection.QueryFirstOrDefault<User>(sqlcommand, param);
                return affectedRow;
            }
        }
    }
    
}
