using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IFixedAssetIncrementDL : IBaseDL<FixedAssetIncrement>
    {
        #region method
        /// <summary>
        ///  Lấy dánh sách tài sản theo mã ghi tăng
        /// </summary>
        /// <param name="id">id của bảng ghi tăng</param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public List<FixedAssetMulti> GetMultiAsset(Guid id, string keyword);

        /// <summary>
        /// Lấy danh sách chi tiết 1 bản ghi 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created:Bùi Quang Điệp(09/10/2022)
        public FixedAssetIncrement GetAssetIncrementDetail(Guid id);

        /// <summary>
        /// Cập nhật nguồn hình thành cho tài sản
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// CreatedBy:Bùi Quang Điệp(10/10/2022)
        public bool UpdateSourceCost(UpdateSourceCost data);

        /// <summary>
        /// Kiểm tra mã trùng
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy : Bùi Quang Điệp(12/10/2022)
        public bool CheckDuplicationCode(FixedAssetIncrement increment,Guid id);

        /// <summary>
        /// TÌm kiếm và phân trang bảng Increment
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy :Bùi Quang Điệp(12/10/2022)
        public PagingData<FixedAssetIncrement> FilterIncrement(string? where, int pageSize = 20, int pageNumber = 1);

        /// <summary>
        /// Lấy tên chứng từ ghi tăng theo mã tài sản
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreateBy :Bùi Quang Điệp (19/10/2022)
        public string GetAssetIncrementName(Guid id);

        #endregion
    }
}
