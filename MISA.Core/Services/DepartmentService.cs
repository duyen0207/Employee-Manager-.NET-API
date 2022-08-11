using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MISA.Core.Models;
using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;

namespace MISA.Core.Services
{
    /// <summary>
    /// Nghiệp vụ liên quan đến phòng ban
    /// </summary>
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        #region Constructor

        public DepartmentService(IDepartmentRepository repository) : base(repository)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Validate phòng ban
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        protected override bool Validate(Department department)
        {
            // TÊN PHÒNG BAN----------------------------------------
            if (!this.NotEmpty(department.DepartmentName, "Tên phòng ban"))
            {
                // kiểm tra trùng lặp
                Console.WriteLine("kiểm tra trùng lặp phòng ban: ", _repository.CheckExistence(department.DepartmentName));
                return _repository.CheckExistence(department.DepartmentName);

            }

            return true;
        }
        #endregion
    }
}
