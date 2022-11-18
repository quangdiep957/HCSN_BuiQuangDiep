using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL.BaseBL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lất tất cả bảng ghi trong 1 bảng
        /// </summary>
        /// <returns>Tất cả bản ghi trong một bảng</returns>
        /// Createed By :Bùi Quang Điệp (23/08/2022)
        public IEnumerable<dynamic> GetAll();

        /// <summary>
        /// Xóa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Xóa 1 bản ghi trong 1 bảng</returns>
        /// createBy : Bùi Quang Điệp (24/08/2022)
        /// 

        public int Delete(List<Guid> ids);


        /// <summary>
        /// Thêm mới 1 bản ghi vào 1 Bảng
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Giá trị cảu bản ghi được thêm mới</returns>
        ///  createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid InsertOneRecord(T record);

        /// <summary>
        /// Tìm kiếm theo mã và tên của từng table
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns> Danh sách bản ghi</returns>
        /// Created By:Bùi Quang Điệp (14/08/2022)
        public IEnumerable<dynamic> SearchRecord( string keyword);


        /// <summary>
        /// Sửa thông tin tài sản
        /// </summary>
        /// <param name="id"> ID tài sản cần chỉnh sửa</param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// Create By:Bùi Quang Điệp( 18/08/2022)
        public Guid UpdateRecord(T record, Guid id);

        /// <summary>
        /// Lấy mã mới nhất
        /// </summary>
        /// <returns></returns>
        /// Create By:Bùi Quang Điệp( 07/10/2022)
        public string GetNewCode();

        /// <summary>
        /// Lấy danh sách 1 bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        //public IEnumerable<dynamic> GetRecordDetail(Guid id);
    }
}
