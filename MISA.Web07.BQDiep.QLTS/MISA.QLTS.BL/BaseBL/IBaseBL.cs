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

        public int Delete(Guid id); 

    }
}
