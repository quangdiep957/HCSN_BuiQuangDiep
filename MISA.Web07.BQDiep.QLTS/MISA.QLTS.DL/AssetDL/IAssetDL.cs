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
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionID"></param>
        /// <param name="departmentID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Danh sách tàn sản</returns>
        ///  Create By : Bùi Quang Điệp (22/08/2022)
        public PagingData<FixedAsset> FilterAsset( string?where, int pageSize = 10, int pageNumber = 1);



        /// <summary>
        /// Lấy thông tin một bản ghi theo mã
        /// </summary>
        /// <param name="id">mã id muốn lấy</param>
        /// <returns>
        /// -200 Kết quả trả về một bản ghi 
        /// -500 Lỗi phía serve
        /// </returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        public FixedAsset GetAssetDetail(Guid id);

        /// <summary>
        /// Kiểm tra xem có mã tài sản này k
        /// </summary>
        /// <returns>Mã tài sản mới nhất</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        public bool CheckDuplicateCode(FixedAsset asset,Guid id);

 
    }
}
