using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Field
        private IUserBL _userBL;
        private readonly IConfiguration _configuration;
        #endregion
        public UsersController(IUserBL userBL, IConfiguration configuration) 
        {
            _userBL = userBL;
            _configuration=configuration;
        } 
        /// <summary>
        /// xử lý sự kiện đăng nhập
        /// </summary>
        /// <param name="user"></param>
        /// <returns>trả về kết quả đằng nhập thành công hay không</returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        [HttpPost("Login")]

        public IActionResult Login(User user)
        {
          var res = _userBL.Login(user);
            if(res != null)
            {
                string token = CreateToken(res, 1);
                // HttpContext.Session.SetString("username", user.UserName);
               // SetJWTCookie(token);

                return new JsonResult(token);


            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, res);
            }
        }

        /// <summary>
        /// tạo token gửi cho FE khi đăng nhập thành công
        /// </summary>
        /// <param name="user"></param>
        /// <returns>trả về token</returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        private string CreateToken(User user,int day)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                 new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(day),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        ///  gán token vào cookie
        /// </summary>
        /// <param name="token"></param>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        private void SetJWTCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1),
                SameSite = SameSiteMode.None

            };
            Response.Cookies.Append("login", token, cookieOptions);
        }


        /// <summary>
        /// Kiểm tra token
        /// </summary>
        /// <returns></returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        [Authorize]
        [HttpGet("checktoken")]
        public bool checkToken()
        {
            return true;
        }

        /// <summary>
        /// xử lý sự kiện logout
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        [Authorize]
        [HttpPost("logout")]
        public IActionResult LogOut(User user)
        {
            string token = CreateToken(user, -1);
            return StatusCode(StatusCodes.Status200OK, token);
        }

    }
}
