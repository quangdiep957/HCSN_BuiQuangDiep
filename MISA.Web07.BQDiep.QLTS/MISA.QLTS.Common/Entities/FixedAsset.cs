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
    [CodeAtrribute("fixedAssetCode")]
    [Key("fixedAssetID")]
    public class FixedAsset
    {
        #region Field
        // camcase var ,paramater,field
        // còn lại dùng PascalCase
        private readonly int id;
        #endregion
        #region Property
        /// <summary>
        /// Id tài sản
        /// </summary>
        /// 

        [MaxLengthText(36)]
        [NameProperty("ID tài sản")]
        public Guid FixedAssetID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// Mã tài sản
        /// </summary>
        /// 
        [MaxLengthText(20)]
        [Required(ErrorMessage = "e006")]
        [NameProperty("Mã tài sản")]

        [NoEmpty]
        public string FixedAssetCode { get; set; }
        /// <summary>
        /// Tên tài sản
        /// </summary>
        /// 
        [NoEmpty]
        [NameAtrribute]
        [MaxLengthText(255)]
        [NameProperty("Tên tài sản")]
        [Required(ErrorMessage = "e007")]
        public string FixedAssetName { get; set; }

        /// <summary>
        /// ID bộ phận sử dụng
        /// </summary>
        /// 
        [Required(ErrorMessage = "e008")]
        [NoEmpty]
        public Guid DepartmentID {get;set;}

        /// <summary>
        /// ID Loại tài sản
        /// </summary>
        /// 
        [Required(ErrorMessage = "e009")]
        [NoEmpty]
        public Guid FixedAssetCategoryID { get; set; }

        /// <summary>
        /// ngày mua
        /// </summary>
        /// 
        [NoEmpty]
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        /// 
        [NoEmpty]
        public decimal Cost { get; set; }

        /// <summary>
        /// số lượng
        /// </summary>
        /// 
        [NoEmpty]
        public decimal Quantity { get; set; }

        /// <summary>
        /// tỉ lệ hao mòn
        /// </summary>
        /// 
        [NoEmpty]
        public decimal DepreciationRate { get; set; }

        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        public decimal TrackedYear { get; set; }

        /// <summary>
        /// số năm sử dụng
        /// </summary>
        public decimal LifeTime { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ngày chỉnh sửa
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// ngày sử dụng
        /// </summary>

        [NoEmpty]
        public DateTime ProductionDate { get; set; }

        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        /// 
        public decimal? DepreciationYear { get; set; }

        /// <summary>
        /// Nguồn hình thành
        /// </summary>
        public string? Price { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        #endregion

    }
}
