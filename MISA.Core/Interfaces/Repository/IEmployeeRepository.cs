using System;
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
        /// Tìm kiếm nhân viên theo tên, số điện thoại, mã
        /// </summary>
        /// <param name="search">cụm từ tìm kiếm</param>
        /// <returns></returns>
        IEnumerable<Object> SearchEmployee(string search);

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex">chỉ mục trang</param>
        /// <param name="pageSize">số bản ghi trên một trang</param>
        /// <param name="employeeFilter">từ khóa tìm kiếm</param>
        /// <returns></returns>
        Object Paging(string? employeeFilter, int pageSize, int pageIndex);

        /// <summary>
        /// Kiểm tra mã nhân viên đã tồn tại hay chưa
        /// </summary>
        /// <param name="EmployeeCode">Mã nhân viên</param>
        /// <returns>danh sách nhân viên tìm được</returns>
        bool CheckExistence(string EmployeeCode);

        /// <summary>
        /// Lấy ra mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns></returns>
        string LatestEmployeeCode();
        #endregion
    }
}
