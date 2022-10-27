using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities.DTO
{
    public class SummaryAsset
    {
        #region property
        /// <summary>
        /// Nguyên giá
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Số lượng    
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Tỉ lệ hao mòn
        /// </summary>
        public decimal DepreciationRate { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public DateTime ProductionDate { get; set; }
        #endregion
    }
}
