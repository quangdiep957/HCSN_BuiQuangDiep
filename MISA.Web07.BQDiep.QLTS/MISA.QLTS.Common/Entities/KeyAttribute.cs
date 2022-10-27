using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MISA.QLTS.Common.Entities
{
    public class KeyAttribute : Attribute
    {
        /// <summary>
        /// khóa
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Hàm khởi tạo keyattribute
        /// </summary>
        /// <param name="key"></param>
        public KeyAttribute(string key)
        {
            Key = key;
        }
    }
}
