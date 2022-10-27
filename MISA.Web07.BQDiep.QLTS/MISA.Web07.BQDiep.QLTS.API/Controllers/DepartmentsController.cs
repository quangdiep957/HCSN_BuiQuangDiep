using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MISA.QLSP.Common.Entities.Entities;
using MISA.QLTS.BL;
using MISA.QLTS.BL.BaseBL;
using MISA.Web07.BQDiep.QLTS.API.BaseController;
using MySqlConnector;

namespace MISA.Web07.BQDiep.QLTS.API.Controllers
{

    public class DepartmentsController : BasesController<Department>
    {
        #region Field
        private IDepartmentBL  _departmentBL;

        public DepartmentsController(IDepartmentBL departmentBL , IMemoryCache memoryCache) : base(departmentBL,memoryCache)
        {
            _departmentBL = departmentBL;
        }
        #endregion
      
        
    }
}
