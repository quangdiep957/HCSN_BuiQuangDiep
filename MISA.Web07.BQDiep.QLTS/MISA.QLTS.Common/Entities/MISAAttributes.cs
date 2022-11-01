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
        /// <summary>
        /// Tên bảng
        /// </summary>
        public string Name;

        /// <summary>
        /// Hàm khởi tạo attribute tên bảng
        /// </summary>
        /// <param name="key"></param>
        public TableName(string name)
        {
            Name = name;
        }
    }
    public class CodeAtrribute : Attribute
    {
        /// <summary>
        /// mã code của bảng
        /// </summary>
        public string Name;

        /// <summary>
        /// Hàm khởi tạo attribute mã 
        /// </summary>
        /// <param name="key"></param>
        public CodeAtrribute(string name)
        {
            Name = name;
        }
    }
    public class NameAtrribute : Attribute
    {
       
    }
    public class NameProperty : Attribute
    {
        /// <summary>
        /// Tên thành phần
        /// </summary>
        public string Name;

        /// <summary>
        /// Hàm khởi tạo attribute tên thành phần
        /// </summary>
        /// <param name="key"></param>
        public NameProperty(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Attribute không được bỏ trống
    /// </summary>
    public class NoEmpty:Attribute{ }

    public class MaxLengthText:Attribute
        { 

        /// <summary>
        /// độ dài tối đa
        /// </summary>
        public int Length;

        /// <summary>
        /// Hàm khởi tạo attribute maxlenght
        /// </summary>
        /// <param name="key"></param>
        public MaxLengthText(int length)
        {
            Length = length;
        }
    }
   
}
