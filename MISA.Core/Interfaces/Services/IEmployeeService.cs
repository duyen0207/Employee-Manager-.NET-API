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
    }
}
