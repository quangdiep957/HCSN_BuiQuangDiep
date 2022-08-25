using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.BL.BaseBL;

namespace MISA.Web07.BQDiep.QLTS.API.BaseController
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field
        private IBaseBL<T> _baseBL;
        #endregion
        #region constructor
        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

      
        #endregion

        #region method

        //<summary>
        //Lấy toàn bộ thông tin 1 bảng
        //</summary>
        // <param name ="Asset">Thông tin 1 bảng </param>
        //<returns>
        // - 200 Lấydữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Created By:Bùi Quang Điệp (14/08/2022)

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {

                var res = _baseBL.GetAll();

                if (res != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
               
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }



        }

        //<summary>
        //Xóa thông tin 1 trường trong 1 bảng
        //</summary>
        // >Xóa thông tin 1 trường trong 1 bảng
        //<returns>
        // - 200 Lấydữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Created By:Bùi Quang Điệp (14/08/2022)

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            try
            {
                int res = _baseBL.Delete(id);
                if (res >=1)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
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
        #endregion
    }
}
