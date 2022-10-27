using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.DL;
using MISA.QLTS.DL.FixedAssetCategoryDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class FixedAssetCategoryBL : BaseBL<FixedAssetCategory>, IFixedAssetCategoryBL
    {
        #region Field
        private IFixedAssetCategoryDL _fixedAssetCategoryDL;
        #endregion

        #region Constructor
        public FixedAssetCategoryBL(IFixedAssetCategoryDL fixedAssetCategoryDL) : base(fixedAssetCategoryDL)
        {
            _fixedAssetCategoryDL = fixedAssetCategoryDL;
        }
        #endregion
    }
}
