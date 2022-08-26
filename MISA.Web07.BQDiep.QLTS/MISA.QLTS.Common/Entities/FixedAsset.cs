using MISA.QLTS.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = MISA.QLTS.Common.Entities.KeyAttribute;

namespace MISA.QLSP.Common.Entities.Entities
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [TableName("fixed_asset")]
    [Key("fixedAssetID")]
    public class FixedAsset
    {
        #region Field
        // camcase var ,paramater,field
        // còn lại dùng PascalCase
        private readonly int id;
        private int MAX_PRICE = 10000;
        #endregion
        #region Property
        /// <summary>
        /// Id tài sản
        /// </summary>
        [Required(ErrorMessage = "e005")]
        public Guid fixedAssetID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Mã tài sản
        /// </summary>
        /// 
        [Required(ErrorMessage = "e006")]
        public string fixedAssetCode { get; set; }
        /// <summary>
        /// Tên tài sản
        /// </summary>
        /// 
        [Required(ErrorMessage = "e007")]
        public string fixedAssetName { get; set; }

        /// <summary>
        /// ID bộ phận sử dụng
        /// </summary>
        [Required(ErrorMessage = "e008")]
        public Guid departmentID {get;set;}

        /// <summary>
        /// ID Loại tài sản
        /// </summary>
        /// 
        [Required(ErrorMessage = "e009")]
        public Guid fixedAssetCategoryID { get; set; }

        /// <summary>
        /// ngày mua
        /// </summary>
        public DateTime purchaseDate { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        public decimal cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao mòn
        /// </summary>
        public float depreciationRate { get; set; }

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        public int trackedYear { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        public int lifeTime { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string? createdBy { get; set; }

        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime createdDate { get; set; }

        /// <summary>
        /// người chỉnh sửa
        /// </summary>
        public string? modifiedBy { get; set; }

        /// <summary>
        /// ngày chỉnh sửa
        /// </summary>
        public DateTime modifiedDate { get; set; }

        /// <summary>
        /// ngày sử dụng
        /// </summary>

        public DateTime productionDate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        public decimal? depreciationYear { get; set; }
        #endregion

    }
}
