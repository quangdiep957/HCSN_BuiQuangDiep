using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entities.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public class AssetBL :BaseDL<FixedAsset>, IAssetBL
    {
        #region Field
        private IAssetDL _assetDL;
        #endregion

        #region Constructor
        public AssetBL(IAssetDL assetDL)
        {
            _assetDL = assetDL;
        }
        #endregion

        #region Method
        //<summary>
        //Lấy toàn bộ thông tin tài sản
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấydữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022

        /// <summary>
        /// Xóa tài sản
        /// </summary>
        /// <param name="fixedAssetID"></param>
        /// <returns>Xóa tài sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        //public int DeleteAsset(Guid fixedAssetID)
        //{
        //    return(_assetDL.DeleteAsset(fixedAssetID));
        //}

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

        //<summary>
        //Thêm mới tài sản
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 201 Thêm mới thành công
        // - 500 có exception
        // / 400 Dữ liệu đầu vào không hợp lệ
        //</returns>
        // Date:14/08/2022
        //public int Post(FixedAsset asset)
        //{
        //    var error = new ErrorSevice();
        //    var errorData = new Dictionary<string, string>();
        //    // 1. Validate dữ liệu
        //    // 1.1 Mã tài sản không được phép để trống 
        //    if (string.IsNullOrEmpty(asset.fixedAssetCode))
        //    {
        //        errorData.Add("FixedAssetCode", BL.Resource.ResourceValidate.AssetCode);
        //    }
        //    // 1.2 Tên tài sản không được phép để trống 
        //    if (string.IsNullOrEmpty(asset.fixedAssetName))
        //    {
        //        errorData.Add("FixedAssetName", BL.Resource.ResourceValidate.AssetName);
        //    }
        //    // 1.3 Mã bộ phận sử dụng không được phép để trống 
        //    //if (string.IsNullOrEmpty(asset.DepartmentCode))
        //    //{
        //    //    errorData.Add("FixedAssetCode", Resource.ResourceValidate.DepartmentCode);
        //    //}
        //    //// 1.4 Mã loại tài sản không được phép để trống 
        //    //if (string.IsNullOrEmpty(asset.FixedAssetCategoryCode))
        //    //{
        //    //    errorData.Add("FixedAssetCode", Resource.ResourceValidate.CategoryCode);
        //    //}
        //    // 1.5 Nguyên giá không được phép để trống 
        //    if (asset.cost == null)
        //    {
        //        errorData.Add("Cost", BL.Resource.ResourceValidate.Cost);
        //    }
        //    // 1.6 Số lượng không được phép để trống 
        //    if (asset.quantity == null)
        //    {
        //        errorData.Add("quantity", BL.Resource.ResourceValidate.Quantity);
        //    }
        //    // 1.7 Ngày mua không được phép để trống 

        //    // 1.8 Ngày bắt đầu sử dụng không được phép để trống 
        //    // 1.9 Tỉ lệ hao mòn không được phép để trống
        //    // 1.10 Giá trị hao mòn không được phép để trống
        //    // 1.11 Số năm sử dụng không được phép để trống
        //    // 2. Kiểm tra độ dài của từng trường
        //    // 3. Validate nghiệp vụ
        //    // 3.1 Tỉ lệ hao mòn trong khoảng từ 1 => tỉ lệ hao mòn = 1/ số năm sử dụng
        //    // 3.2 Hao mòn năm phải nhỏ hơn hoặc bằng nguyên giá => Hao mòn / khấu hao năm > nguyên giá

        //    if (errorData.Count > 0)
        //    {
        //        //error.UserMsg = BL.Resource.ResourceVN.Error_ValidateData;
        //        //error.data = errorData;
               
        //    }
        //   return _assetDL.Post(asset);
        //}

        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="FixedAssetID"> ID tài sản cần chỉnh sửa</param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Date: 18/08/2022
        public int UpdateAsset(Guid FixedAssetID, FixedAsset asset)
        {
            return (_assetDL.UpdateAsset(FixedAssetID, asset));
        }


        public string GetCodeAsset()
        {
            var assetNew = "";
            var assetCode = _assetDL.GetNewAsset();
            ////  tách chuỗi thành số
            if (assetCode != "")
            {
                var resultString = Regex.Match(assetCode.ToString(), @"\d{3}").Value;
                int number = int.Parse(resultString);
                number = number + 1;
                assetNew = "TS" + number;
            }
            else
            {
                assetNew = "TS001";
            }
            return assetNew;
        }

        //public IEnumerable<FixedAsset> Get()
        //{
        //    var res = _assetDL.Get();
        //    return res;
        //}

        public PagingData<FixedAsset> FilterAsset(string? keyword, Guid? categoryAssetID, Guid? departmentID, int pageSize = 20, int pageNumber = 1)
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

            if (andConditions.Count > 0)
            {
                if(orConditions.Count > 0)
                {
                    whereClause += $"AND {string.Join(" AND ", andConditions)}";
                } 
                else
                whereClause += $" {string.Join(" AND ", andConditions)}";
            }
            return _assetDL.FilterAsset(whereClause,pageSize,pageNumber);
        }






        #endregion
    }
}
