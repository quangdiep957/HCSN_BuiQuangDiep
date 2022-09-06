using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IAssetDL:IBaseDL<FixedAsset>
    {
        /// <summary>
        /// Lấy tất cả tài sản
        /// <returns>tất cả tài sản</returns>
        /// Created by: Bùi Quang Điệp (20/08/2022)
       // IEnumerable<FixedAsset> Get();

        /// <summary>
        /// Lấy mã tài sản mới nhất
        /// <returns>mã tài sản  mới nhất</returns>
        /// Created by: Bùi Quang Điệp (20/08/2022)

       public string GetNewAsset();

        ///<summary>
        ///Thêm mới tài sản
        ///</summary>
        /// <param name ="asset">Thông tin tài sản </param>
        ///<returns>
        /// - 201 Thêm mới thành công
        /// - 500 có exception
        /// 400 Dữ liệu đầu vào không hợp lệ
        ///</returns>
        /// Date:14/08/2022
        //int Post(FixedAsset asset);

        ///// <summary>
        ///// Sửa thông tin tài sản
        ///// </summary>
        ///// <param name="AssetId"> ID tài sản cần chỉnh sửa</param>
        ///// <param name="asset"></param>
        ///// <returns></returns>
        ///// Date: 18/08/2022
        int UpdateAsset(Guid fixedAssetID, FixedAsset asset);

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionID"></param>
        /// <param name="departmentID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Danh sách tàn sản</returns>
        public PagingData<FixedAsset> FilterAsset( string?where, int pageSize = 10, int pageNumber = 1);

        /// <summary>
        /// Xóa tài sản
        /// </summary>
        /// <param name="fixedAssetID"></param>
        /// <returns>Xóa nhiều tài sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        int DeleteAssetMulti(List<Guid> fixedAssetID);

        public FixedAsset GetOneAsset(Guid id);
    }
}
