using Dapper;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.Common.Entities.DTO;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public interface IAssetBL:IBaseBL<FixedAsset>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// Date: 18/08/2022
        public string GetPagingAsset(string ID);

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
        public PagingData<FixedAsset> FilterAsset(string? keyword, Guid? categoryAssetID, Guid? departmentID, List<Guid> dataFixedAssetID, int pageSize = 10, int pageNumber = 1,int status=2);

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

    }
}
