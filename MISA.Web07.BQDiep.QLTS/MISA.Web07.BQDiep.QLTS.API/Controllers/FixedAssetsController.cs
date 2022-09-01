using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.Web07.BQDiep.QLTS.API.BaseController;
using MySqlConnector;
using System.Data;
using System.Text.RegularExpressions;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetsController : BasesController<FixedAsset>
    {
        #region Field
        private IAssetBL _assetBL;
        #endregion
        public FixedAssetsController(IAssetBL assetBL) : base(assetBL)
        {
            _assetBL = assetBL;
        }
       

        ////<summary>
        ////Lấy mã tài sản mới nhất
        ////</summary>
        //// <param name ="Asset">Thông tin tài sản </param>
        ////<returns>
        //// - 200 Lấy dữ liệu thành công
        //// - 500 Lỗi từ phía sever
        ////</returns>
        //// Author:14/08/2022
        [HttpGet("CodeAsset")]
        public IActionResult GetCodeAsset()
        {

            try
            {
                //// 1 Khai báo thông tin Database
                //var connectionString = "Host=localhost;Database=misa.web07.hcsn.diep;port=3306;User Id=root;password=Quangdiep@2001";

                //// 2 Khởi tạo kết nối tới Mysql
                //var sqlConection = new MySqlConnection(connectionString);

                //// 3 Lấy dữ liệu
                //var sqlCommand = "SELECT MAX(FixedAssetCode) FROM fixed_asset";
                //var assetCode = sqlConection.QueryFirst<object>(sql: sqlCommand);
                //////  tách chuỗi thành số
                //if (assetCode != "")
                //{
                //    var resultString = Regex.Match(assetCode.ToString(), @"\d{3}").Value;
                //    int number = int.Parse(resultString);
                //    number = number + 1;
                //    assetNew = "TS" + number;
                //}
                //else
                //{
                //    assetNew = "TS001";
                //}
                 var assetNew = _assetBL.GetCodeAsset();

                // 4 Trả về kết quả
                return Ok(assetNew);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }



        }

        //// <sumary>
        //// tìm kiếm tài sản
        ////</summary>


        ////<summary>
        ////Thêm mới tài sản
        ////</summary>
        //// <param name ="Asset">Thông tin tài sản </param>
        ////<returns>
        //// - 201 Thêm mới thành công
        //// - 500 có exception
        //// / 400 Dữ liệu đầu vào không hợp lệ
        ////</returns>
        //// Date:14/08/2022
        //[HttpPost]

        //public IActionResult Post([FromBody] FixedAsset asset)
        //{
        //    try
        //    {

        //        // 4 Trả về kết quả
        //        var res = _assetBL.Post(asset);
        //        if (res > 0)
        //        {
        //            return StatusCode(StatusCodes.Status201Created, asset);
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
        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="AssetId"> ID tài sản cần chỉnh sửa</param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Date: 18/08/2022
        [HttpPut]

        public IActionResult UpdateAsset(Guid FixedAssetID, [FromBody] FixedAsset asset)
        {
            try
            {


                var res = _assetBL.UpdateAsset(FixedAssetID, asset);
                // 4 Trả về kết quả
                if (res > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, asset);
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
            return StatusCode(500, error);
        }

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionID"></param>
        /// <param name="departmentID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Danh sách tàn sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        [HttpGet("filter")]
        public IActionResult FilterAsset([FromQuery] string? keyword, [FromQuery] Guid? categoryAssetID, [FromQuery] Guid? departmentID, [FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1)
        {
            try
            {
                var res = _assetBL.FilterAsset(keyword, categoryAssetID, departmentID, pageSize, pageNumber);

                if (res != null)
                {
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }
           

        }

        /// <summary>
        /// Xóa nhiều bản ghi
        /// </summary>
        /// <param name="fixedassetids"></param>
        /// <returns>Số lượng bản ghi đã xóa</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        [HttpPost("deleteMulti")]
        public IActionResult DeleteAssetMulti([FromBody] List<Guid> fixedassetids)
        {
            try
            {
                var res = _assetBL.DeleteAssetMulti(fixedassetids);
                if (res > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, res);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, res);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }


        }



    }
}
