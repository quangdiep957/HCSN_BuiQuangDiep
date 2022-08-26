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
        public string Key { get; set; }
        public KeyAttribute(string key)
        {
            Key = key;
        }
    }
}
