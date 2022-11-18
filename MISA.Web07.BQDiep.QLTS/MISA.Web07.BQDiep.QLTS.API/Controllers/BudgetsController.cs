using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.QLTS.BL;
using MISA.QLTS.Common.Entities;
using MISA.Web07.BQDiep.QLTS.API.BaseController;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BudgetsController : BasesController<Budget>
    {
        #region Field
        private IBudgetBL _budgetBL;

        public BudgetsController(IBudgetBL budgetBL) : base(budgetBL)
        {
            _budgetBL = budgetBL;
        }
        #endregion
    }
}
