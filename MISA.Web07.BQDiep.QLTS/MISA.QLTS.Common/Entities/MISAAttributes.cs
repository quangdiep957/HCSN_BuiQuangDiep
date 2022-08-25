using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName:Attribute
    {
        public string Name;
        public TableName(string name)
        {
            Name = name;
        }
    }
}
