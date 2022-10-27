
using MISA.QLTS.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLSP.Common.Entities.Entities
{
    [TableName("fixed_asset_category")]
    [Key("FixedAssetCategoryID")]
    [CodeAtrribute("FixedAssetCategoryCode")]
    public class FixedAssetCategory
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>

        #region propperty
        [NoEmpty]
        public Guid FixedAssetCategoryID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// mã loại tài sản
        /// </summary>
        /// 
        [NoEmpty]
     
        public string FixedAssetCategoryCode { get; set; }
        /// <summary>
        /// tên loại tài sản
        /// </summary>
        /// 
        [NoEmpty]
        [NameAtrribute]
        public string FixedAssetCategoryName { get; set; }
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
        #endregion

    }
}
