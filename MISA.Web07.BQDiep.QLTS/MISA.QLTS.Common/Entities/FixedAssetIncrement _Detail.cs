using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities
{
    public class FixedAssetIncrement_Detail
    {
        #region property
        /// <summary>
        /// ID ghi tăng tài sản
        /// </summary>
        public Guid FixedAssetIncrementID { get; set; }

        /// <summary>
        /// ID tài sản
        /// </summary>
        public Guid FixedAssetAssetID { get; set; }
        #endregion
    }
}
