using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.Web07.BQDiep.QLTS.API.BaseController;
using MySqlConnector;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    public class FixedAssetCategorysController : BasesController<FixedAssetCategory>
    {
        #region Field
        private IFixedAssetCategoryBL  _fixedAssetCategoryBL;
        #endregion

    #region Constructor
            public FixedAssetCategorysController(IFixedAssetCategoryBL fixedAssetCategoryBL) : base(fixedAssetCategoryBL)
            {
            _fixedAssetCategoryBL = fixedAssetCategoryBL;
            }
        #endregion
       
    }

}
