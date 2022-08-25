using MISA.QLTS.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class Department
    {
        /// <summary>
        /// ID bộ phận sử dụng
        /// </summary>
        [Key]
        public Guid departmentID { get; set; }
        /// <summary>
        /// Mã bộ phận sử dụng
        /// </summary>
        public string departmentCode { get; set; }
        /// <summary>
        /// tên bộ phận sử dụng
        /// </summary>
        public string departmentName { get; set; }
        /// <summary>
        /// mô tả
        /// </summary>
        public string note { get; set; }
        /// <summary>
        /// người tạo
        /// </summary>
        public string createdBy { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime createdDate { get; set; }
        /// <summary>
        /// người chỉnh sửa
        /// </summary>
        public string modifiedBy { get; set; }
        /// <summary>
        /// ngày chỉnh sửa
        /// </summary>
        public DateTime modifiedDate { get; set; }

    }
}
