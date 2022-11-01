using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using MISA.Web07.BQDiep.QLTS.API.BaseController;
using MySqlConnector;
using System.Collections.Generic;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetIncrementsController : BasesController<FixedAssetIncrement>
    {
        #region Field
        private IFixedAssetIncrementBL _fixedAssetIncrementBL;
        public readonly IMemoryCache _memoryCache;

        public FixedAssetIncrementsController(IFixedAssetIncrementBL fixedAssetIncrementBL, IMemoryCache memoryCache) : base(fixedAssetIncrementBL, memoryCache)
        {
            _fixedAssetIncrementBL = fixedAssetIncrementBL;
            _memoryCache = memoryCache;

        }

        #endregion

        #region method 

        /// <summary>
        /// Lấy danh sách tài sản theo mã ghi tăng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="filterID"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// CreatedBy : Bùi Quang Điệp(16/10/2022)
        [HttpPost("AssetMulti")]
        public IActionResult GetMultiAsset(Guid? id, string? key, string? keyword, int filterID = (int)Filter.None)
        {
            try
            {
                var cacheValue = new List<Guid>();
                if (filterID != (int)Filter.None)
                {
                    cacheValue = (List<Guid>)_memoryCache.Get(key);
                    if (cacheValue != null)
                    {
                        if (cacheValue.Count == 0)
                            cacheValue = new List<Guid>();
                        else
                        {
                            cacheValue = (List<Guid>)_memoryCache.Get(key);
                        }
                    }

                }
                if (id == null)
                {
                    id = Guid.Empty;
                }
                var record = _fixedAssetIncrementBL.GetMultiAsset((Guid)id, cacheValue, keyword);
                    return StatusCode(StatusCodes.Status200OK, record);
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
        /// Lấy danh sách chi tiết 1 bản ghi 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        [HttpGet("IncrementDetail")]
        public IActionResult GetAssetIncrementDetail(Guid id)
        {
            try
            {


                var record = _fixedAssetIncrementBL.GetAssetIncrementDetail(id);
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e004);
                }
            }

            catch (Exception ex)
            {

                return HandleException(ex);

            }
        }

        /// <summary>
        /// Lấy tên chứng từ ghi tăng theo mã tài sản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreateBy :Bùi Quang Điệp (19/10/2022)
        [HttpGet("GetNameIncremnt")]
        public IActionResult GetAssetIncrementName(Guid id)
        {
            try
            {


                var record = _fixedAssetIncrementBL.GetAssetIncrementName(id);
                if (record != null)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e004);
                }
            }

            catch (Exception ex)
            {

                return HandleException(ex);

            }
        }

        /// <summary>
        /// Cập nhật nguồn hình thành cho tài sản
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// CreatedBy:Bùi Quang Điệp(10/10/2022)
        [HttpPut("UpdateCost")]
        public IActionResult UpdateSourceCost([FromBody] UpdateSourceCost data)
        {
            try
            {
                var record = _fixedAssetIncrementBL.UpdateSourceCost(data);
                if (record)
                {
                    return StatusCode(StatusCodes.Status200OK, record);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Error.e004);
                }
            }

            catch (Exception ex)
            {

                return HandleException(ex);

            }
        }

        /// <summary>
        /// TÌm kiếm theo mã và nội dung bảng ghi tăng
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="categoryAssetID"></param>
        /// <param name="departmentID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// CreatedBy:Bùi Quang Điệp(12/10/2022)
        [HttpPost("filter")]
        public IActionResult FilterIncrement([FromQuery] string? keyword, [FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var res = _fixedAssetIncrementBL.FilterIncrement(keyword, pageSize, pageNumber);

                if (res != null)
                {
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
            }
            catch (MySqlException msql)
            {
                var error = new ErrorSevice();
                error.UserMsg = "Không được chứa các kí tự đặc biệt !";
                return HandleException(error);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        ///// <summary>
        ///// Hiển thị lỗi cho người dùng
        ///// </summary>
        ///// <param></param>
        ///// <returns>Danh sách lỗi
        /// Created By:Bùi Quang Điệp (14/08/2022)
        private IActionResult HandleException(Exception ex)
        {

            var error = new ErrorSevice();
            error.Handle = (int)Handler.Bussiness;
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
            return StatusCode(500, error);
        }
        #endregion
    }
}
