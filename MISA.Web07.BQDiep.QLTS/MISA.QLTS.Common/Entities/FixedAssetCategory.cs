
using MISA.QLTS.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLSP.Common.Entities.Entities
{
    [TableName("fixed_asset_category")]
    [Key("FixedAssetCategoryID")]
    public class FixedAssetCategory
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
   
        #region propperty
        public Guid FixedAssetCategoryID { get; set; } = Guid.NewGuid();
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
