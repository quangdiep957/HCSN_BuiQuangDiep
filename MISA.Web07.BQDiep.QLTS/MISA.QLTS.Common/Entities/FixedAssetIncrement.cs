using MISA.QLTS.Common.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities
{
    [TableName("fixed_asset_increment")]
    [CodeAtrribute("FixedAssetIncrementCode")]
    [Key("fixedAssetIncrementID")]
    public class FixedAssetIncrement
    {
        #region property
     /// <summary>
     /// ID ghi tăng tài sản
     /// </summary>
        public Guid FixedAssetIncrementID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã ghi tăng tài sản
        /// </summary>
        [NoEmpty]
        [MaxLengthText(20)]
        [NameProperty("Mã chứng từ ghi tăng")]
        public string FixedAssetIncrementCode { get; set; }

        /// <summary>
        /// Tên ghi tăng tài sản
        /// </summary>
        public string? FixedAssetIncrementName { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        [NoEmpty]
        [NameProperty("Ngày chứng từ")]
        public DateTime DateVocher { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        [NoEmpty]
        [NameProperty("Ngày ghi tăng")]
        public DateTime DateIncrement { get; set; }

        /// <summary>
        /// ghi chú
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        [NoEmpty]
        public decimal Price { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// Ngày chỉnh sửa
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        ///  Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Danh sách ID tài sản 
        /// </summary>
        [NoEmpty]
        public List<Guid> FixedAssetIDs { get; set; }

        /// <summary>
        /// Danh sách tài sản 
        /// </summary>
        [NoEmpty]
        public List<UpdateSourceCost>? FixedAssets { get; set; }
        #endregion
    }
}
