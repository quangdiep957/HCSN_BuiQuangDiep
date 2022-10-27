using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities.DTO
{
    public class UpdateSourceCost
    {
        #region property
        /// <summary>
        /// ID ghi tăng tài sản
        /// </summary>
        public Guid FixedAssetIncrementID { get; set; }

        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid FixedAssetID { get; set; }

        /// <summary>
        /// Danh sách nguồn hình thành
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Tổng nguyên giá
        /// </summary>
        public decimal Sumcost { get; set; }
        #endregion
    }
}
