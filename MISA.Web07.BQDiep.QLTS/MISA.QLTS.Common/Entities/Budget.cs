using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.Common.Entities
{
    public class Budget
    {
        #region property
        /// <summary>
        /// ID nguồn hình thành
        /// </summary>
        public Guid BudgetID { get; set; }

        /// <summary>
        /// Tên nguồn hình thành
        /// </summary>
        public string BudgetName { get; set; }

        /// <summary>
        /// nguyên giá
        /// </summary>
        public decimal Mount { get; set; }

        /// <summary>
        /// Mã nguồn hình thành
        /// </summary>
        public string BudgetCode { get; set; }
      
        #endregion
    }
}
