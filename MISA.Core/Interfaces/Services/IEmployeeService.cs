using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MISA.Core.Models;

namespace MISA.Core.Interfaces.Services
{
    public interface IEmployeeService:IBaseService<Employee>
    {
        /// <summary>
        /// Tự động tạo mã nhân viên mới
        /// </summary>
        /// <param name="maxLoop">giới hạn vòng lặp lấy mã nhân viên mới khi mã bị trùng</param>
        /// <returns></returns>
        string GetNewCode(int maxLoop = 10);

        /// <summary>
        /// Phân trang danh sách nhân viên
        /// </summary>
        /// <param name="pageSize">số lượng bản ghi trên một trang</param>
        /// <param name="pageNumber">số trang</param>
        /// <param name="employeeFilter">nội dung tìm kiếm</param>
        /// <returns>TotalPages, TotalRecords, Data </returns>
        Object Paging(string? employeeFilter, int pageSize, int pageIndex = 1);
    }
}
