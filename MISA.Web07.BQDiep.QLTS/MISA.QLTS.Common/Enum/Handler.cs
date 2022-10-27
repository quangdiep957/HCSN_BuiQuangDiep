using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Enum
{
    public enum Handler
    {
        /// <summary>
        ///  lỗi chưa nhập đầy đu dữ liệu
        /// </summary>
        Required = 1,

        /// <summary>
        /// Lỗi trùng mã
        /// </summary>
        DoubleKey= 2,

        /// <summary>
        /// Lỗi exception 
        /// </summary>
        Exception = 3,

        /// <summary>
        /// Lỗi logic nghiệp vụ 
        /// </summary>
        Bussiness = 4

    }
    public enum Query
    {
        /// <summary>
        /// Chỉnh sửa
        /// </summary>
        update = 1,

        /// <summary>
        /// Xóa
        /// </summary>
        delete = 2,

        /// <summary>
        /// Thêm mới
        /// </summary>
        create = 3

    }
}

