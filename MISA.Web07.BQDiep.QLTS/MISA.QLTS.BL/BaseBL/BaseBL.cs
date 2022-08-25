﻿using MISA.QLTS.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region constructor
        public BaseBL(IBaseDL<T> baseDL){
            _baseDL = baseDL;
          }

        public int Delete(Guid id)
        {
          return _baseDL.Delete(id);
        }
        #endregion

        #region method
        /// <summary>
        /// Lất tất cả bảng ghi trong 1 bảng
        /// </summary>
        /// <returns>Tất cả bản ghi trong một bảng</returns>
        /// Createed By :Bùi Quang Điệp (23/08/2022)
        public IEnumerable<dynamic> GetAll()
        {
            return _baseDL.GetAll();
        }

        #endregion
    }
}
