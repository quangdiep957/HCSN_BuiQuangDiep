using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class User
    {
        /// <summary>
        /// ID người dùng
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// mật khẩu
        /// </summary>
        public string Password { get; set; }
    }
}
