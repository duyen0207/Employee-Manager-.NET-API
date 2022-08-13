﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MISA.Core.Models;

namespace MISA.Core.Interfaces.Repository
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        #region Methods

        /// <summary>
        /// Kiểm tra mã nhân viên đã tồn tại hay chưa
        /// </summary>
        /// <param name="EmployeeCode">Mã nhân viên</param>
        /// <returns></returns>
        bool CheckExistence(string EmployeeCode);

        /// <summary>
        /// Lấy ra mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns></returns>
        string LatestEmployeeCode();
        #endregion
    }
}
