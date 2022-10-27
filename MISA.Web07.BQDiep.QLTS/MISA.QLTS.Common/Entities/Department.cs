using MISA.QLTS.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLSP.Common.Entities.Entities
{
    [Key("departmentID")]
    [CodeAtrribute("departmentCode")]
    public class Department
    {
        /// <summary>
        /// ID bộ phận sử dụng
        /// </summary>
        /// 
        [Required]
        [NoEmpty]
        public Guid DepartmentID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Mã bộ phận sử dụng
        /// </summary>
        /// 

        [NoEmpty]
      
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
        [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        public string DepartmentCode { get; set; }
        /// <summary>
        /// tên bộ phận sử dụng
        /// </summary>
        /// 
        [NoEmpty]
        [NameAtrribute]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
        [StringLength(maximumLength: 25, MinimumLength = 10, ErrorMessage = "Length must be between 10 to 25")]
        public string DepartmentName { get; set; }
        /// <summary>
        /// mô tả
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// người chỉnh sửa
        /// </summary>
        public string ModifiedBy { get; set; }
        /// <summary>
        /// ngày chỉnh sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

    }
}
