using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class UserBL: IUserBL
    {

        #region Field
        private IUserDL _userDL;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public UserBL(IUserDL userDL, IConfiguration configuration)
        {
            _userDL = userDL;
            _configuration = configuration;
        }

        /// <summary>
        /// xử lý sự kiện đăng nhập
        /// </summary>
        /// <param name="user"></param>
        /// <returns>trả về kết quả đằng nhập thành công hay không</returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        public User Login(User user)
        {
           return _userDL.Login(user);
        }

        
        #endregion
    }
}
