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
        //<summary>
        //Lấy toàn bộ thông tin tài sản
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấydữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022

       // public IEnumerable<FixedAsset> Get();

        //<summary>
        //Lấy mã tài sản mới nhất
        //</summary>
        // <param name ="Asset">Thông tin tài sản </param>
        //<returns>
        // - 200 Lấy dữ liệu thành công
        // - 500 Lỗi từ phía sever
        //</returns>
        // Author:14/08/2022
        public string GetCodeAsset();

        // <sumary>
        // tìm kiếm tài sản
        //</summary>


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

       // public int Post(FixedAsset asset);
        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="AssetId"> ID tài sản cần chỉnh sửa</param>
        /// <param name="asset"></param>
        /// <returns></returns>
        /// Date: 18/08/2022

        public int UpdateAsset(Guid FixedAssetID, FixedAsset asset);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// Date: 18/08/2022

        public string GetPagingAsset(string ID);


        /// <summary>
        /// Xóa tài sản
        /// </summary>
        /// <param name="fixedAssetID"></param>
        /// <returns>Xóa tài sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        //public int DeleteAsset(Guid fixedAssetID);

        /// <summary>
        /// Hàm thông báo lỗi cho người dùng
        /// </summary>
        /// <param name="ex"> Bắt lỗi</param>
        /// <returns></returns>
        //private IActionResult HandleException(Exception ex);

        /// <summary>
        /// Tìm kiếm và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="positionID"></param>
        /// <param name="departmentID"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>Danh sách tàn sản</returns>
        public PagingData<FixedAsset> FilterAsset(string? keyword, Guid? categoryAssetID, Guid? departmentID, int pageSize = 10, int pageNumber = 1);

        /// <summary>
        /// Xóa tài sản
        /// </summary>
        /// <param name="fixedAssetID"></param>
        /// <returns>Xóa nhiều tài sản</returns>
        /// Create By : Bùi Quang Điệp (22/08/2022)
        public int DeleteAssetMulti(List<Guid> fixedAssetID);

        public FixedAsset GetOneAsset(Guid id);
    }
}
