using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using MISA.QLTS.Common.Enum;
using MISA.QLTS.Common.Resource;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class FixedAssetIncrementBL:BaseBL<FixedAssetIncrement>,IFixedAssetIncrementBL
    {
        #region Field
        private IFixedAssetIncrementDL _fixedAssetIncrementDL;
        #endregion

        #region constructor
        public FixedAssetIncrementBL(IFixedAssetIncrementDL fixedAssetIncrementDL) :base(fixedAssetIncrementDL)
        {
            _fixedAssetIncrementDL = fixedAssetIncrementDL;
        }

        /// <summary>
        /// Lấy danh sách chi tiết 1 bản ghi 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public FixedAssetIncrement GetAssetIncrementDetail(Guid id)
        {
            return _fixedAssetIncrementDL.GetAssetIncrementDetail(id);
        }

        /// <summary>
        ///  Lấy dánh sách tài sản theo mã ghi tăng
        /// </summary>
        /// <param name="id">id của bảng ghi tăng</param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public List<FixedAssetMulti> GetMultiAsset(Guid id, string keyword)
        {
            return _fixedAssetIncrementDL.GetMultiAsset(id,keyword);
        }

        /// <summary>
        /// Cập nhật nguồn hình thành cho tài sản
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// CreatedBy:Bùi Quang Điệp(10/10/2022)
        public bool UpdateSourceCost(UpdateSourceCost data)
        {
            return _fixedAssetIncrementDL.UpdateSourceCost(data);
        }

        public string GetAssetIncrementName(Guid id)
        {
            return _fixedAssetIncrementDL.GetAssetIncrementName(id);
        }

        /// <summary>
        /// Check mã trùng khi update dữ liệu
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="id"></param>
        /// CreatedBy : Bùi Quang Điệp(12/10/2022)
        protected override void ValidateCheckUpdate(FixedAssetIncrement increment, Guid id)
        {


            var checkDoubleCode = _fixedAssetIncrementDL.CheckDuplicationCode(increment, id);
            if (!checkDoubleCode)
            {
                var error = new ErrorSevice();
                var errorsData = new List<string>();
                error.Handle = (int)Handler.DoubleKey;
                error.DevMsg = ResourceVN.Error_ValidateData;
                error.UserMsg = $"{increment.FixedAssetIncrementCode} " + ResourceValidate.DoubleKey;
                errorsData.Add(increment.FixedAssetIncrementCode);
                error.DataError = errorsData;
                throw error;
            }
        }

        /// <summary>
        /// Check mã trùng khi thêm mới
        /// </summary>
        /// <param name="asset"></param>
        /// Created By:Bùi Quang Điêp(19/09/2022)
        protected override void ValidateCheckCreate(FixedAssetIncrement increment)
        {

            var checkDoubleCode = _fixedAssetIncrementDL.CheckDuplicationCode(increment, Guid.Empty);
            if (!checkDoubleCode)
            {
                var error = new ErrorSevice();
                var errorsData = new List<string>();
                error.Handle = (int)Handler.DoubleKey;
                error.DevMsg = ResourceVN.Error_ValidateData;
                error.UserMsg = $"{increment.FixedAssetIncrementCode} " + ResourceValidate.DoubleKey;
                errorsData.Add(increment.FixedAssetIncrementCode);
                error.DataError = errorsData;
                throw error;
            }

        }

        /// <summary>
        /// TÌm kiếm và phân trang bảng Increment
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy :Bùi Quang Điệp(12/10/2022)
        public PagingData<FixedAssetIncrement> FilterIncrement(string? keyword, int pageSize = 10, int pageNumber = 1)
        {

            var orConditions = new List<string>();
            string whereClause = "";

            if (keyword != null)
            {
                orConditions.Add($"FixedAssetIncrementCode LIKE '%{keyword}%'");
                orConditions.Add($"Description LIKE '%{keyword}%'");
            }
            if (orConditions.Count > 0)
            {
                whereClause = $"({string.Join(" OR ", orConditions)})";
            }
            return _fixedAssetIncrementDL.FilterIncrement(whereClause, pageSize, pageNumber);
        }
        
        /// <summary>
        /// Validate dùng riêng cho bảng ghi tăng tài sản
        /// </summary>
        /// <param name="assetIncrement"></param>
        /// CreateBy : Bùi Quang Điệp (11/01/2022)
        protected override void ValidateRecord(FixedAssetIncrement assetIncrement)
        {
            var error = new ErrorSevice();
            var errorsData = new List<string>();
             // Kiểm tra cần chọn ít nhất 1 tài sản
             if(assetIncrement.FixedAssetIDs.Count == 0)
            {
                errorsData.Add(ResourceValidate.MinAsset);
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
