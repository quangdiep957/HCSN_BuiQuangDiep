using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities.DTO
{
    public class FixedAssetIncrementDTO
    {
        #region property
        /// <summary>
        /// ID ghi tăng tài sản
        /// </summary>
        public Guid FixedAssetIncrementID { get; set; }

        /// <summary>
        /// Mã ghi tăng tài sản
        /// </summary>
        public string FixedAssetIncrementCode { get; set; }

        /// <summary>
        /// Tên ghi tăng tài sản
        /// </summary>
        public string FixedAssetncrementName { get; set; }

        /// <summary>
        /// Ngày ghi tăng
        /// </summary>
        public DateTime DateVocher { get; set; }

        /// <summary>
        /// Ngày chứng từ
        /// </summary>
        public DateTime DateIncrement { get; set; }

        /// <summary>
        /// ghi chú
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// Id tài sản
        /// </summary>
        public Guid FixedAssetAssetID { get; set; }
        #endregion
    }
}
