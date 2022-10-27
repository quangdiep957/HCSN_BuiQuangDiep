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
        public string FixedAssetCode { get; set; }
        /// <summary>
        /// Tên tài sản
        /// </summary>
        /// 
        [Required(ErrorMessage = "e007")]
        public string FixedAssetName { get; set; }
        /// <summary>
        /// ID bộ phận sử dụng
        /// </summary>

        public Guid DepartmentID { get; set; }
        /// <summary>
        /// ID Loại tài sản
        /// </summary>
        public Guid FixedAssetCategoryID { get; set; }
        /// <summary>
        /// ngày mua
        /// </summary>
        public DateTime PurchaseDate { get; set; }
        /// <summary>
        /// nguyên giá
        /// </summary>
        public decimal Cost { get; set; }
        /// <summary>
        /// số lượng
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// tỉ lệ hao mòn
        /// </summary>
        public float DepreciationRate { get; set; }
        /// <summary>
        /// năm bắt đầu theo dõi
        /// </summary>
        public int TrackedYear { get; set; }
        /// <summary>
        /// số năm sử dụng
        /// </summary>
        public int LifeTime { get; set; }
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

        public DateTime ProductionDate { get; set; }
        /// <summary>
        /// giá trị hao mòn năm
        /// </summary>
        public decimal? DepreciationYear { get; set; }
    #endregion

    
}
}
