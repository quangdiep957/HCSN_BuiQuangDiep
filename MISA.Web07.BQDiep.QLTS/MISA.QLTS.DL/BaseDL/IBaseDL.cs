using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// Lấy tất cả bản ghi của 1 bảng
        /// </summary>
        /// <returns>tất cả bản ghi của 1 bảng</returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)
        public IEnumerable<dynamic> GetAll();

        /// <summary>
        /// Xóa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Xóa 1 bản ghi trong 1 bảng</returns>
        /// createdBy : Bùi Quang Điệp (24/08/2022)

        public int Delete(Guid id);

        /// <summary>
        /// Thêm mới 1 bản ghi vào 1 Bảng
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Giá trị cảu bản ghi được thêm mới</returns>
        ///  /// createdBy : Bùi Quang Điệp (24/08/2022)
        public Guid InsertOneRecord(T record);


        /// <summary>
        /// Sửa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <param name=""></param>
        /// <param name="id"></param>
        /// <returns>Trả về mã bản ghi đã được sửa</returns>
        public Guid UpdateOneRecord(T record,Guid id);

        /// <summary>
        /// Danh sách một bản ghi
        /// </summary>
        /// <returns>Danh sách một bản ghi</returns>
        /// /// Createed By :Bùi Quang Điệp (23/08/2022)
        //public IEnumerable<dynamic> GetOneRecord(Guid id);


    }
}
