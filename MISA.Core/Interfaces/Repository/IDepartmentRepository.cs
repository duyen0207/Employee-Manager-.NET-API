using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MISA.Core.Models;

namespace MISA.Core.Interfaces.Repository
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        /// <summary>
        /// tìm id của department theo tên
        /// </summary>
        /// <param name="departmentName">tên phòng ban</param>
        /// <returns>id của phòng ban</returns>
        object GetId(string departmentName);
    }
}
