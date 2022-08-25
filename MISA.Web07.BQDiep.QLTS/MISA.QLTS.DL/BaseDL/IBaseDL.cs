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
        /// createBy : Bùi Quang Điệp (24/08/2022)
        public IEnumerable<dynamic> GetAll();

        /// <summary>
        /// Xóa 1 bản ghi trong 1 bảng
        /// </summary>
        /// <returns>Xóa 1 bản ghi trong 1 bảng</returns>
        /// createBy : Bùi Quang Điệp (24/08/2022)

        public int Delete(Guid id);


    }
}
