using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MISA.Core.Models;
using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Exceptions;
using System.Text.RegularExpressions;

namespace MISA.Core.Services
{
    /// <summary>
    /// Nghiệp vụ liên quan đến nhân viên
    /// </summary>
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        #region Properties
        // _employeeRepository: dùng để truy cập employee database
        // _departmentReposity: dùng để truy cập department database
        IEmployeeRepository _employeeRepository;
        IDepartmentRepository _departmentRepository;

        #endregion

        #region Constructor

        public EmployeeService(IEmployeeRepository employeeReposity, IDepartmentRepository departmentRepository) : base(employeeReposity)
        {
            _employeeRepository = employeeReposity;
            _departmentRepository = departmentRepository;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Tạo mã nhân viên mới
        /// </summary>
        /// Lưu ý: mã nhân viên phải có ít nhất 4 số
        /// <returns></returns>
        public string GetNewCode(int maxLoop = 3)
        {
            maxLoop--;
            // lấy ra mã của nhân viên mới nhất được thêm vào
            string latestCode = _employeeRepository.LatestEmployeeCode();
            // nếu chưa có nhân viên nào trong cơ sở dữ liệu
            if (string.IsNullOrEmpty(latestCode)) return "NV-0001";
            // tách lấy số
            int codeNumber = Convert.ToInt32(Regex.Match(latestCode, @"\d+").Value);
            // tạo mã nhân viên mới
            string newCode = "NV-" + String.Format("{0:D4}", codeNumber + 1);

            // kiểm tra nếu trùng thì gọi hàm một lần nữa
            if (_employeeRepository.CheckExistence(newCode) == true)
            {
                if (maxLoop > 0) return GetNewCode(maxLoop);
                else throw new MISAValidateException("Mã nhân viên mới tạo trùng nhiều lần. Bạn nên tạo thủ công mã mới");
            }

            return newCode;
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        protected override bool Validate(Employee employee)
        {
            // Check trường bắt buộc

            /// MÃ NHÂN VIÊN-----------------------------------------
            if(!this.CheckEmpty(employee.EmployeeCode, "Mã nhân viên"))
            {
                // kiểm tra trùng
                if(!_employeeRepository.CheckExistence(employee.EmployeeCode))
                {
                    // kiểm tra hợp lệ (vd: mã phải chứa số và chữ...)
                    if (!Regex.IsMatch(employee.EmployeeCode, @"^NV-[0-9]{4,}"))
                        throw new MISAValidateException("Mã nhân viên không hợp lệ. Mã nhân viên phải có dạng NV-<chuỗi số ít nhất 4 ký tự>");

                } else throw new MISAValidateException("Mã nhân viên đã tồn tại trong hệ thống.");
            }

            /// TÊN NHÂN VIÊN----------------------------------------
            this.CheckEmpty(employee.FullName, "Tên");

            /// PHÒNG BAN--------------------------------------------
            if (!this.CheckEmpty(employee.DepartmentId.ToString(), "Phòng ban"))
            {
                // kiểm tra phòng ban có tồn tại trong csdl hay không
                var department = _departmentRepository.Get(employee.DepartmentId.ToString());
                if (department == null)
                    throw new MISAValidateException("Phòng ban không tồn tại.");
            }
            
            // NGÀY THÁNG----------------------------------------------
            /// ngày sinh
            ValidateDate(employee.DateOfBirth.ToString(), "Ngày sinh");

            /// ngày cấp cmnd
            ValidateDate(employee.IdentityDate.ToString(), "Ngày cấp CMND");

            // EMAIL ---------------------------------------------------
            ValidateEmail(employee.Email);

            return true;
        }

        /// <summary>
        /// Check email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    throw new MISAValidateException("Email không hợp lệ.");
            }
            return true;
        }

        /// <summary>
        /// Check ngày tháng hợp lệ
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool ValidateDate(string dateTime, string dateName="Ngày")
        {
            if(!string.IsNullOrEmpty(dateTime))
            {
                DateTime date;
                // check ngày hợp lệ
                if (DateTime.TryParse(dateTime, out date))
                {
                    // check ngày không được vượt quá thời điểm hiện tại
                    if (DateTime.Compare(date, DateTime.Today) > 0)
                        throw new MISAValidateException($"{dateName} không được lớn hơn thời điểm hiện tại");

                }
                else throw new MISAValidateException($"{dateName} không hợp lệ");

            }

            return true;
        }

        #endregion

    }
}
