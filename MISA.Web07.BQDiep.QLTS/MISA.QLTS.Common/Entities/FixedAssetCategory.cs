
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.QLSP.Common.Entities.Entities
{
    public class FixedAssetCategory
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        [Key]
        #region propperty
        public string FixedAssetCategoryID { get; set; }
        /// <summary>
        /// mã loại tài sản
        /// </summary>
        public string FixedAssetCategoryCode { get; set; }
        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public string FixedAssetCategoryName { get; set; }
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
        #endregion

    }
}
