using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.Common.Resource;
using MySqlConnector;
using System.Reflection.Metadata;
using System.Web.Http.ModelBinding;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;


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
                    return StatusCode(StatusCodes.Status500InternalServerError, Error.e002);
                }

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }



        }

        /// <summary>
        /// Tìm kiếm theo mã và tên của từng table
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns> Danh sách bản ghi</returns>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        [HttpGet("search")]
        public IActionResult SearchRecord(string keyword)
        {
            try
            {

                var res = _baseBL.SearchRecord(keyword);

                if (res != null)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e015);
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
        public IActionResult Delete(List<Guid> id)
        {
            try
            {
                int res = _baseBL.Delete(id);
                if (res >= 1)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, Error.e002);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Thêm mới dữ liệu cảu một bảng
        /// </summary>
        /// <param name="record"> tên bảng</param>
        /// <returns>
        /// - 201 Thêm mới thành công
        //  - 500 Lỗi từ phía sever
        /// </returns>
        ///  Created By:Bùi Quang Điệp (14/08/2022)
        [Authorize]
        [HttpPost]
        public IActionResult InsertOneRecord([FromBody] T record)
        {

            try
            {
                var recordID = _baseBL.InsertOneRecord(record);
                if (recordID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e004);
                }
            }
            catch (ErrorSevice ex)
            {
                return HandleExceptionError(ex);
            }
            catch (Exception ex)
            {

                return HandleException(ex);


            }
        }

        /// <summary>
        /// Sửa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <param name="record"></param>
        /// <param name="id"></param>
        ///   /// <param name="fixedAssetyCode"> Mã tài sản đầu tiên của bản ghi đó</param>
        /// <returns>Trả về mã bản ghi đã được sửa</returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        [HttpPut]
        public IActionResult UpdateRecord([FromBody] T record, Guid FixedAssetID, string fixedAssetCode)
        {
            try
            {
                var recordID = _baseBL.UpdateRecord(record, FixedAssetID);
                if (recordID != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, recordID);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e004);
                }
            }
            catch (ErrorSevice ex)
            {
                return HandleExceptionError(ex);
            }
            catch (Exception ex)
            {

                return HandleException(ex);


            }
        }

        /// <summary>
        /// Lấy danh sách 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
//        [HttpGet("Detail")]
//        public IActionResult GetRecordDetail(Guid id)
//        {

//            try
//            {
//                var res = _baseBL.GetRecordDetail(id);
//                if (res != null)
//                {
//                    // Trả về dữ liệu cho client
//                    return StatusCode(StatusCodes.Status200OK, res);
//                }
//                else
//                {
//                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
//                }
//            }
//            catch (Exception ex)
//            {
//                return HandleException(ex);
//            }
        
            
    
//}

        ///// <summary>
        ///// Hiển thị lỗi cho người dùng
        ///// </summary>
        ///// <param></param>
        ///// <returns>Danh sách lỗi
        /// Created By:Bùi Quang Điệp (14/08/2022)
        private IActionResult HandleException(Exception ex)
        {

            var error = new ErrorSevice();
            error.Handle = 4;
            error.UserMsg = ResourceVN.Error_Exception;
            error.DevMsg = ex.Message;
            return StatusCode(500, error);
        }

        ///// <summary>
        ///// Hiển thị lỗi cho người dùng
        ///// </summary>
        ///// <param></param>
        ///// <returns>Danh sách lỗi
        /// Created By:Bùi Quang Điệp (14/08/2022)
        private IActionResult HandleExceptionError(ErrorSevice ex)
        {

            var error = new ErrorSevice();

            error.Handle = ex.Handle;
            error.UserMsg = ex.UserMsg;
            error.DevMsg = ex.DevMsg;
            error.DataError = ex.DataError;
            return StatusCode(400, error);
        }

        //<summary>
        //Lấy mã tài sản mới nhất
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấy dữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022 Bùi Quang Điệp
        [HttpGet("CodeAsset")]
        public IActionResult GetNewCode()
        {
            string assetNew = "";
            try
            {
                assetNew = _baseBL.GetNewCode();
                return Ok(assetNew);
            }
            catch (Exception ex)
            {
                if (ex.Message == ResourceNewCode.NoData)
                {
                    assetNew = ResourceNewCode.NewCode;
                    return Ok(assetNew);
                }
                return HandleException(ex);
            }



        }
        #endregion
    }
}
