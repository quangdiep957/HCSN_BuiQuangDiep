using System.ComponentModel.DataAnnotations;

namespace MISA.QLSP.Entities.DTO
{
    public class FixedAssetDTO
    {
        /// <summary>
        /// Mã tài sản
        /// </summary>
        /// 
         #region property
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

        public Guid departmentID { get; set; }
        /// <summary>
        /// ID Loại tài sản
        /// </summary>
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
