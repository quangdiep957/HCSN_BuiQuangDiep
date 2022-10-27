using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Dapper;
using MISA.QLTS.Common.Resource;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using MISA.QLTS.Common.Enum;

namespace MISA.QLTS.DL
{
    public class AssetBL : BaseBL<FixedAsset>, IAssetBL
    {
        #region Field
        private IAssetDL _assetDL;
        #endregion

        #region Constructor
        public AssetBL(IAssetDL assetDL) : base(assetDL)
        {
            _assetDL = assetDL;
        }


        #endregion

        #region Method
      
        //<summary>
        //Lấy mã tài sản mới nhất
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấy dữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022
        //public int IEnumerable<FixedAsset>()
        //{
        //    return _assetDL.GetCodeAsset();
        //}

        public string GetPagingAsset(string ID)
        {
            throw new NotImplementedException();
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
        public PagingData<FixedAsset> FilterAsset(string? keyword, Guid? categoryAssetID, Guid? departmentID, List<Guid> dataFixedAssetID, int pageSize = 20, int pageNumber = 1,int status=2)
        {

            var orConditions = new List<string>();
            var andConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                orConditions.Add($"FixedAssetID LIKE '%{keyword}%'");
                orConditions.Add($"FixedAssetname LIKE '%{keyword}%'");
                orConditions.Add($"FixedAssetCode LIKE '%{keyword}%'");

            }
            if (dataFixedAssetID.Count > 0)
            {
                var assetID = "(";
                foreach (var data in dataFixedAssetID)
                {
                    assetID = assetID + "'" + data + "'" + ",";
                }
                assetID = assetID.Remove(assetID.Length - 1) + ")";
                orConditions.Add($"FixedAssetID NOT IN{assetID}");
            }

            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
            }

            if (categoryAssetID != null)
            {
                andConditions.Add($"FixedAssetCategoryID LIKE '%{categoryAssetID}%'");
            }
            if (departmentID != null)
            {
                andConditions.Add($"DepartmentID LIKE '%{departmentID}%'");
            }
            if (status < 2)
            {
                andConditions.Add($"Status = {status}");
            }
           
            if (andConditions.Count > 0)
            {
                if (orConditions.Count > 0)
                {
                    whereClause += $" AND {string.Join(" AND ", andConditions)}";
                }
                else
                    whereClause += $" {string.Join(" AND ", andConditions)}";
            }
           
            return _assetDL.FilterAsset(whereClause, pageSize, pageNumber);
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
        public FixedAsset GetAssetDetail(Guid id)
        {
            return _assetDL.GetAssetDetail(id);
        }

        ///// <summary>
        ///// Check mã trùng khi update
        ///// </summary>
        ///// <param name="asset"></param>
        ///// <param name="id"></param>
        ///// Created By : Bùi Quang Điệp (18/09/2022)
        protected override void ValidateCheckUpdate(FixedAsset asset,Guid id)
        {
                var checkDuplicateCode = _assetDL.CheckDuplicateCode(asset,id);
                if (!checkDuplicateCode)
                {
                    var error = new ErrorSevice();
                    var errorsData = new List<string>();
                    error.Handle = (int)Handler.DoubleKey;
                    error.DevMsg = ResourceVN.Error_ValidateData;
                    error.UserMsg = $"{asset.FixedAssetCode} " + ResourceValidate.DoubleKey;
                    errorsData.Add(asset.FixedAssetCode);
                    error.DataError = errorsData;
                    throw error;
                }
            }

        /// <summary>
        /// Check mã trùng khi thêm mới
        /// </summary>
        /// <param name="asset"></param>
        /// Created By:Bùi Quang Điêp(19/09/2022)
        protected override void ValidateCheckCreate(FixedAsset asset)
        {

            var checkDuplicateCode = _assetDL.CheckDuplicateCode(asset,Guid.Empty);
            if (!checkDuplicateCode)
            {
                var error = new ErrorSevice();
                var errorsData = new List<string>();
                error.Handle = (int)Handler.DoubleKey;
                error.DevMsg = ResourceVN.Error_ValidateData;
                error.UserMsg = $"{asset.FixedAssetCode} " + ResourceValidate.DoubleKey;
                errorsData.Add(asset.FixedAssetCode);
                error.DataError = errorsData;
                throw error;
            }

        }

        /// <summary>
        /// Check validate asset
        /// </summary>
        /// <param name="asset"></param>
        /// Created By : Bùi Quang Điệp (18/09/2022)
        protected override void ValidateRecord(FixedAsset asset)
        {
            var error = new ErrorSevice();
            var errorsData = new List<string>();
            if (asset.PurchaseDate > DateTime.Now)
            {

                errorsData.Add(ResourceValidate.PurchaseDate);


            }
            if (asset.ProductionDate > DateTime.Now)
            {
                errorsData.Add(ResourceValidate.DateUse);
            }
            const int separatorNumber = 2;
            const int percent = 100;
            const int oneYear = 100;
            decimal depreciationRate = Math.Round(asset.DepreciationRate, separatorNumber);
            decimal comparativeDepreciationRate = oneYear / (decimal)oneYear / asset.LifeTime * percent;
             comparativeDepreciationRate = Math.Round(comparativeDepreciationRate, separatorNumber);
            if (depreciationRate != comparativeDepreciationRate)
            {
                errorsData.Add(ResourceValidate.professional1);
            }

            if ((decimal)asset.DepreciationRate / asset.DepreciationYear > asset.Cost)
            {
                errorsData.Add(ResourceValidate.professional2);
            }
            if (errorsData.Count > 0)
            {
                error.Handle = (int)Handler.Required;
                error.DevMsg = ResourceVN.Error_ValidateData;
                error.UserMsg = ResourceValidate.Required;
                error.DataError = errorsData;
                throw error;
            }
        }
        #endregion
    }
}
