using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MySqlConnector;
using System.Web.Http.ModelBinding;


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

        [HttpPost("delete")]
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

        [HttpPost]
        public IActionResult InsertOneRecord([FromBody] T record)
        {

            try
            {

                var validateResult = HandleError.HandleError.IsValidate(ModelState);
                if (validateResult != null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, validateResult);
                }

                var checkType = typeof(T).Name;
                if (checkType == "FixedAsset")
                {
                    var asset = record as FixedAsset;
                    var validateProfessional = HandleError.HandleError.checkProfessional(asset);

                    if(validateProfessional != null)
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, validateProfessional);
                    }    
                }    
                   
               
                 var recordID= _baseBL.InsertOneRecord(record);
                if (recordID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e004");
                }
            }
            catch (MySqlException mySqlException)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.HandleError.GenerateDuplicateCodeErrorResult(mySqlException));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Sửa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <param name=""></param>
        /// <param name="id"></param>
        /// <returns>Trả về mã bản ghi đã được sửa</returns>
        //public IActionResult UpdateRecord([FromBody] T record, Guid id)
        //{
        //    return StatusCode(4000);
        //}

        private IActionResult HandleException(Exception ex)
        {
            var error = new ErrorSevice();
            error.UserMsg = Resource.ResourceVN.Error_Exception;
            error.DevMsg = ex.Message;
            return StatusCode(500, error);
        }

        ///// <summary>
        ///// Lấy danh sách một bản ghi 
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns>Danh sách một bản ghi</returns>
        //[HttpGet("Detail")]
        
        //public IActionResult GetOneRecord(Guid id)
        //{
        //    try
        //    {
        //        var res = _baseBL.GetOneRecord(id);
        //        if (res !=null)
        //        {
        //            // Trả về dữ liệu cho client
        //            return StatusCode(StatusCodes.Status200OK, res);
        //        }
        //        else
        //        {
        //            return StatusCode(StatusCodes.Status400BadRequest, "e002");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}
        #endregion
    }
}
