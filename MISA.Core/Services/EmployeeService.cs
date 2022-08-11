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
    /// Nghiệp vụ liên quan đến nhân viên
    /// </summary>
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Properties
        // _employeeRepository: dùng để truy cập employee database
        IEmployeeRepository _employeeRepository;

        #endregion

        #region Constructor

        public EmployeeService(IEmployeeRepository repository) : base(repository)
        {
            _employeeRepository = repository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        protected override bool Validate(Employee employee)
        {
            // Check trường bắt buộc

            /// MÃ NHÂN VIÊN-----------------------------------------
            if(!this.NotEmpty(employee.EmployeeCode, "Mã nhân viên"))
            {
                // kiểm tra trùng
                if(!_employeeRepository.CheckExistence(employee.EmployeeCode))
                {
                    // kiểm tra hợp lệ (vd: mã phải chứa số và chữ...)

                }
            }


            // TÊN NHÂN VIÊN----------------------------------------
            if (!this.NotEmpty(employee.FullName, "Tên"))
            {
                // kiểm tra tên hợp lệ
                
            }
            // PHÒNG BAN--------------------------------------------
            if (!this.NotEmpty(employee.DepartmentId.ToString(), "Phòng ban"))
            {
                // kiểm tra phòng ban có tồn tại trong csdl hay không
                
            }

            // NGÀY THÁNG----------------------------------------------

            // ngày sinh

            // ngày cấp cmnd

            // EMAIL ---------------------------------------------------


            return true;
        }

        /// <summary>
        /// Check email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool validateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {

            }
            return true;
        }

        /// <summary>
        /// Tạo mã nhân viên mới
        /// </summary>
        /// Lưu ý: mã nhân viên phải có ít nhất 4 số
        /// <returns></returns>
        public string GetNewCode()
        {
            // lấy ra mã của nhân viên mới nhất được thêm vào
            var latestCode = _employeeRepository.LatestEmployee().EmployeeCode;
            // tách lấy số và thêm 0 ở đầu nếu cần thiết

            // tạo mã nhân viên mới

            // kiểm tra nếu trùng

            return null;
        }


        #endregion

    }
}
