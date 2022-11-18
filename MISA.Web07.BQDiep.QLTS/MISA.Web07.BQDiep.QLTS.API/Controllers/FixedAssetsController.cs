using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using MISA.QLTS.DL;
using MISA.Web07.BQDiep.QLTS.API.BaseController;
using MySqlConnector;
using OfficeOpenXml;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using static MISA.Web07.BQDiep.QLTS.API.Controllers.CachesController;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FixedAssetsController : BasesController<FixedAsset>
    {
        #region Field
        private IAssetBL _assetBL;
        public readonly IMemoryCache _memoryCache;
        #endregion
        public FixedAssetsController(IAssetBL assetBL) : base(assetBL)
        {
            _assetBL = assetBL;
        }




        /// <summary>
        /// Bắt lỗi exception
        /// </summary>
        /// <param name="ex">Lỗi trả về</param>
        /// <returns>
        /// mã lỗi
        /// </returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        private IActionResult HandleException(Exception ex)
        {
            var error = new ErrorSevice();
            error.UserMsg = ResourceVN.Error_Exception;
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
        [HttpPost("filter")]
        public IActionResult FilterAsset([FromQuery] string? keyword, [FromQuery] Guid? categoryAssetID, [FromQuery] Guid? departmentID, [FromQuery] int pageSize = 20, [FromQuery] int pageNumber = 1, [FromQuery] int status = 2, int filterID = (int)Filter.None ,List<Guid> cacheValue = null)
        {
            try
            {
                //var items = new List<Guid>();
                //var cacheValue = new List<Guid>();
                //if (filterID != (int)Filter.None)
                //{
                //    var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
                //    var collection = field.GetValue(_memoryCache) as ICollection;

                //    if (collection != null)
                //        foreach (var item in collection)
                //        {
                //            var methodInfo = item.GetType().GetProperty("Key");
                //            var val = methodInfo.GetValue(item);
                //            var dataCache = (List<Guid>)_memoryCache.Get(val);
                //            foreach (var data in dataCache)
                //            {
                //                items.Add(data);
                //            }
                //        }

                //}

                var res = _assetBL.FilterAsset(keyword, categoryAssetID, departmentID, cacheValue, pageSize, pageNumber, status);

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


        /// <summary>
        /// Lấy thông tin một bản ghi theo mã
        /// </summary>
        /// <param name="id">mã id muốn lấy</param>
        /// <returns>
        /// -200 Kết quả trả về một bản ghi 
        /// -500 Lỗi phía serve
        /// </returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        /// 
        [HttpGet("Detail")]

        public IActionResult GetAssetDetail(Guid id)
        {
            try
            {
                var res = _assetBL.GetAssetDetail(id);
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

        /// <summary>
        /// Import Excel
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// -200 Kết quả trả về một bản ghi 
        /// -500 Lỗi phía serve
        /// </returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        ///
        [HttpPost("ImportExcel")]
        public IActionResult UploadData(IFormFile file)
        {

            if (file?.Length > 0)
            {
                // convert to a stream
                var stream = file.OpenReadStream();

                List<FixedAsset> assets = new List<FixedAsset>();

                try
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;

                        for (var row = 2; row <= rowCount; row++)
                        {

                            var assetID = worksheet.Cells[row, 1].Value;
                            var assetCode = worksheet.Cells[row, 2].Value?.ToString();
                            var assetName = worksheet.Cells[row, 3].Value?.ToString();
                            var departmentID = worksheet.Cells[row, 4].Value;
                            var categoryID = worksheet.Cells[row, 5].Value;
                            var datePure = worksheet.Cells[row, 6].Value;
                            var cost = worksheet.Cells[row, 7].Value;
                            var quantity = worksheet.Cells[row, 8].Value;
                            var rate = worksheet.Cells[row, 9].Value;
                            var year = worksheet.Cells[row, 10].Value;
                            var yearUse = worksheet.Cells[row, 11].Value;
                            var createdBy = worksheet.Cells[row, 12].Value?.ToString();
                            var createdDate = worksheet.Cells[row, 13].Value;
                            var modifiedBy = worksheet.Cells[row, 14].Value?.ToString();
                            var modifiedDate = worksheet.Cells[row, 15].Value;
                            var productionDate = worksheet.Cells[row, 16].Value;
                            var depreciationYear = worksheet.Cells[row, 17].Value;

                            // valide check mã tài sản đã có chưa
                            // check mã bộ phận có hay không
                            // Check loại tài sản đó đã có chưa

                            var asset = new FixedAsset();

                            asset.FixedAssetID = (Guid)assetID;
                            asset.FixedAssetCode = assetCode;
                            asset.FixedAssetName = assetName;
                            asset.DepartmentID = (Guid)departmentID;
                            asset.FixedAssetCategoryID = (Guid)categoryID;
                            asset.PurchaseDate = (DateTime)datePure;
                            asset.Cost = (decimal)cost;
                            asset.Quantity = (decimal)quantity;
                            asset.DepreciationRate = (decimal)rate;
                            asset.TrackedYear = (decimal)year;
                            asset.LifeTime = (decimal)yearUse;
                            asset.CreatedBy = createdBy;
                            asset.CreatedDate = (DateTime)createdDate;
                            asset.ModifiedBy = modifiedBy;
                            asset.ModifiedDate = (DateTime)modifiedDate;
                            asset.ProductionDate = (DateTime)productionDate;
                            asset.DepreciationYear = (decimal)depreciationYear;


                            assets.Add(asset);

                            if (assets.Count > 0)
                            {
                                var sqlCommand = "INSERT INTO fixed_asset values";

                                foreach (var ase in assets)
                                {
                                    sqlCommand = sqlCommand + '(' + $"{asset.FixedAssetID}" + ',' + $"{asset.FixedAssetCode}" + ',' + $"{asset.FixedAssetName}" + ',' + $"{asset.DepartmentID}" + ',' + $"{asset.FixedAssetCategoryID}" + ',' + $"{asset.PurchaseDate}" + ',' + $"{asset.Cost}" + ',' + $"{asset.Quantity}" + ',' + $"{asset.DepreciationRate}" + ',' + $"{asset.TrackedYear}" + ',' + $"{asset.LifeTime}" + ',' + $"{asset.CreatedBy}" + ',' + $"{asset.CreatedDate}" + ',' + $"{asset.ModifiedBy}" + ',' + $"{asset.ModifiedDate}" + ',' + $"{asset.DepreciationYear}" + ')';
                                }
                                using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                                {
                                    sqlConnection.Open();

                                    using (var trans = sqlConnection.BeginTransaction())
                                    {
                                        var response = sqlConnection.Execute(sqlCommand);

                                        trans.Commit();

                                        if ((int)response > 0)
                                        {
                                            return StatusCode(StatusCodes.Status200OK);
                                        }
                                        else
                                        {
                                            return StatusCode(StatusCodes.Status400BadRequest);
                                        }
                                    }


                                }

                            }

                        }
                    }
                    return StatusCode(StatusCodes.Status200OK);
                }
                catch (Exception ex)
                {
                    return HandleException(ex);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}