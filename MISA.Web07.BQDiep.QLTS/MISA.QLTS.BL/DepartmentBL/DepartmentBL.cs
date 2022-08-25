using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL.BaseBL;
using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public class DepartmentBL:BaseBL<Department>,IDepartmentBL
    {
        #region Field
        private IDepartmentDL _departmentDL;
        #endregion

        #region constructor
        public DepartmentBL(IDepartmentDL departmentDL) :base(departmentDL)
        {
            _departmentDL = departmentDL;
        }
        #endregion
    }
}
