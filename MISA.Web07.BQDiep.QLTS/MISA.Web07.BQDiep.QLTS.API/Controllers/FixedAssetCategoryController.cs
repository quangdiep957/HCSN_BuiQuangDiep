using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLSP.Common.Entities.Entities;
using MySqlConnector;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetCategoryController : ControllerBase
    {
        //<summary>
        //Lấy toàn bộ thông tin bộ phận sử dụng
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấy dữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022
        [HttpGet]
        public IActionResult get()
        {
            try
            {
                // 1 Khai báo thông tin Database
                var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

                // 2 Khởi tạo kết nối tới Mysql
                var sqlConection = new MySqlConnection(connectionString);

                // 3 Lấy dữ liệu
                var sqlCommand = "SELECT FixedAssetCategoryCode,FixedAssetCategoryName ,FixedAssetCategoryID FROM fixed_asset_category";
                var departments = sqlConection.Query<object>(sql: sqlCommand);

                // 4 Trả về kết quả
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        private IActionResult HandleException(Exception ex)
        {
            var error = new ErrorSevice();
            error.UserMsg = Resource.ResourceVN.Error_Exception;
            error.DevMsg = ex.Message;
            error.data = ex.Data;
            return StatusCode(500, error);
        }

    }

}
