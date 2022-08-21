using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        Object Paging(string? employeeFilter, int pageSize=10, int pageIndex = 1);

        /// <summary>
        /// Nhập khẩu dữ liệu
        /// </summary>
        /// <param name="importFile">tệp nhập khẩu</param>
        /// <returns>danh sách nhân viên trong tệp nhập khẩu kèm trạng thái đã nhập khâu hay chưa
        /// </returns>
        /// Created by: LTBDUYEN (2022)
        Object Import(IFormFile importFile);
    }
}
