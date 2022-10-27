﻿using MISA.QLSP.Common.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.QLTS.BL
{
    public interface IUserBL
    {
        /// <summary>
        /// xử lý sự kiện đăng nhập
        /// </summary>
        /// <param name="user"></param>
        /// <returns>trả về kết quả đằng nhập thành công hay không</returns>
        /// Created By : Bùi Quang Điệp(26/09/2022)
        public User Login(User user);  
    }
}