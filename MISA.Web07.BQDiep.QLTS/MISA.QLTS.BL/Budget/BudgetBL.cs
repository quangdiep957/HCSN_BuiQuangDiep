using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.Common.Entities;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class BudgetBL:BaseBL<Budget>,IBudgetBL
    {
        #region Field
        private IBudgetDL _budgetDL;
        #endregion

        #region Constructor
        public BudgetBL(IBudgetDL budgetDL) : base(budgetDL)
        {
            _budgetDL = budgetDL;
        }
        #endregion
    }
}
