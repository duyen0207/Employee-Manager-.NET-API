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
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

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
        readonly IEmployeeRepository _employeeRepository;
        readonly IDepartmentRepository _departmentRepository;

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
        /// Phân trang danh sách nhân viên
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="employeeFilter"></param>
        /// <returns></returns>
        public Object Paging(string? employeeFilter, int pageSize=10, int pageIndex=1)
        {
            var res = _employeeRepository.Paging(employeeFilter, pageSize, pageIndex);
            return res;
        }


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
                // nếu là thêm mới hoặc sửa employee code
                if( employee.EmployeeId == Guid.Empty || 
                    (employee.EmployeeId != Guid.Empty && 
                     employee.EmployeeCode != _employeeRepository.Get(employee.EmployeeId.ToString()).EmployeeCode
                    )
                ) {
                    // kiểm tra trùng
                    if (!_employeeRepository.CheckExistence(employee.EmployeeCode))
                    {
                        // kiểm tra hợp lệ (vd: mã phải chứa số và chữ...)
                        if (!Regex.IsMatch(employee.EmployeeCode, @"^NV-[0-9]{4,}"))
                        {
                            IsValid = false;
                            ValidateErrorMsgs.Add("Mã nhân viên không hợp lệ. Mã nhân viên phải có dạng NV-<chuỗi số ít nhất 4 ký tự>");
                        }
                    }
                    else
                    {
                        IsValid = false ;
                        ValidateErrorMsgs.Add("Mã nhân viên đã tồn tại trong hệ thống.");
                    }
                } 
            }

            /// TÊN NHÂN VIÊN----------------------------------------
            this.CheckEmpty(employee.FullName, "Tên");

            /// PHÒNG BAN--------------------------------------------
            if (!this.CheckEmpty(employee.DepartmentId.ToString(), "Phòng ban"))
            {
                // kiểm tra phòng ban có tồn tại trong csdl hay không
                var department = _departmentRepository.Get(employee.DepartmentId.ToString());
                if (department == null)
                {
                    IsValid = false;
                    ValidateErrorMsgs.Add("Phòng ban không tồn tại.");
                }
            }
            
            // NGÀY THÁNG----------------------------------------------
            /// ngày sinh
            ValidateDate(employee.DateOfBirth.ToString(), "Ngày sinh");

            /// ngày cấp cmnd
            ValidateDate(employee.IdentityDate.ToString(), "Ngày cấp CMND");

            // EMAIL ---------------------------------------------------
            ValidateEmail(employee.Email);

            return IsValid;
        }

        /// <summary>
        /// Check email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ValidateEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    IsValid = false;
                    ValidateErrorMsgs.Add("Email không hợp lệ.");
                    return false;

                }    
            }
            return true;
        }

        /// <summary>
        /// Check ngày tháng hợp lệ
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public bool ValidateDate(string dateTime, string dateName="Ngày")
        {
            if(!string.IsNullOrEmpty(dateTime))
            {
                DateTime date;
                // check ngày hợp lệ
                if (DateTime.TryParse(dateTime, out date))
                {
                    // check ngày không được vượt quá thời điểm hiện tại
                    if (DateTime.Compare(date, DateTime.Today) > 0)
                    {
                        IsValid = false;
                        ValidateErrorMsgs.Add($"{dateName} không được lớn hơn thời điểm hiện tại");
                        return false;
                    }

                }
                else
                {
                    IsValid=false;
                    ValidateErrorMsgs.Add($"{dateName} không hợp lệ");
                    return false;
                }

            }

            return true;
        }

        /// <summary>
        /// Nhập khẩu dữ liệu
        /// </summary>
        /// <param name="importFile">tệp nhập khẩu</param>
        /// <returns>danh sách nhân viên trong tệp nhập khẩu kèm trạng thái đã nhập khâu hay chưa
        /// </returns>
        /// Created by: LTBDUYEN (2022)
        public Object Import(IFormFile importFile)
        {
            // validate tệp
            if (importFile == null || importFile.Length <= 0)
            {
            }

            if (!Path.GetExtension(importFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new MISAValidateException("Tệp không đúng định dạng.");
            }

            var employees = new List<Employee>();
            int success = 0;

            using (var stream = new MemoryStream())
            {
                importFile.CopyToAsync(stream);

                // đọc tệp excel
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    // tự động quét được vùng dữ liệu (vùng được bôi đen khung)
                    var rowCount = worksheet.Dimension.Rows;

                    // duyệt từng dòng một trong sheet (bắt đầu từ dòng thứ hai vì dòng một là tiêu đề)
                    for (int row = 2; row <= rowCount; row++)
                    {

                        var employeeCode = ConvertToString(worksheet.Cells[row, 2].Value);
                        var fullName = ConvertToString(worksheet.Cells[row, 3].Value);
                        var departmentName = ConvertToString(worksheet.Cells[row, 4].Value);

                        var departmentId = ConvertToGuid(_departmentRepository.GetId(departmentName));

                        var positionName = ConvertToString(worksheet.Cells[row, 5].Value);
                        var gender = GetGenderCode(ConvertToString(worksheet.Cells[row, 6].Value));

                        //var date = worksheet.Cells[row, 7].Value;
                        var dateOfBirth = ConvertToDate(worksheet.Cells[row, 7].Value);
                        var identityNumber = ConvertToString(worksheet.Cells[row, 8].Value);
                        var identityDate = ConvertToDate(worksheet.Cells[row, 9].Value);
                        var identityBy = ConvertToString(worksheet.Cells[row, 10].Value);
                        
                        var email = ConvertToString(worksheet.Cells[row, 11].Value);
                        var phoneNumber = ConvertToString(worksheet.Cells[row, 12].Value);
                        var telephoneNumber = ConvertToString(worksheet.Cells[row, 13].Value);
                         
                        var bankAccountNumber = ConvertToString(worksheet.Cells[row, 14].Value);
                        var bankName = ConvertToString(worksheet.Cells[row, 15].Value);
                        var bankBranchName = ConvertToString(worksheet.Cells[row, 16].Value);

                        var employee = new Employee
                        {
                            EmployeeCode = employeeCode,
                            DepartmentId = departmentId,
                            FullName = fullName,
                            DateOfBirth = dateOfBirth,
                            Gender = gender,

                            IdentityNumber = identityNumber,
                            IdentityDate = identityDate,
                            IdentityBy = identityBy,

                            Email = email,
                            PhoneNumber = phoneNumber,
                            TelephoneNumber = telephoneNumber,

                            BankAccountNumber = bankAccountNumber,
                            BankBranchName = bankBranchName,
                            BankName = bankName
                        };

                        // validate dữ liệu
                        // xóa toàn bộ thông tin đã validate trước đó
                        ResetValidate();

                        var isValid = Validate(employee);
                        
                        if(!isValid)
                        {
                            employee.IsValidImport = false;
                            if(ValidateErrorMsgs!=null) employee.ImportError.AddRange(ValidateErrorMsgs);
                        } else
                        {
                            success++;
                            // thêm mới vào db
                            InsertService(employee);
                        }
                        employees.Add(employee);
                    }
                }
                return new
                {
                    SuccessRecords= $"{success}/{employees.Count()} records",
                    Details = employees.Select(e => new { e.EmployeeCode, e.FullName, e.IsValidImport, e.ImportError })
                };
                    
            }
        }

        #region Import, Export Preprocessing

        /// <summary>
        /// Chuyển giới tính về dạng số để lưu trong database
        /// </summary>
        /// <param name="genderName">giới tính</param>
        /// <returns>Nam: 0, Nữ: 1, Khác: 3</returns>
        public int? GetGenderCode(string genderName)
        {
            if (string.IsNullOrEmpty(genderName)) return null;
            else if (genderName == "Nam") return 0;
            else if (genderName == "Nữ") return 1;
            else return 3;
        }
        

        /// <summary>
        /// Chuyển giá trị sang chuỗi
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string? ConvertToString(object? value)
        {
            if (value != null) return value.ToString();
            return null;
        }

        /// <summary>
        /// Chuyển sang kiểu dữ liệu DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime? ConvertToDate(object value)
        {
            double num;
            if (value != null)
            {
                try
                {
                    num = Convert.ToDouble(value);
                    return DateTime.FromOADate(num);

                }
                catch (Exception)
                {

                    return null;
                }
                
            }
            return null;
        }
        #endregion

        public Guid ConvertToGuid(object value)
        {
            if (value == null) return Guid.Empty;
            else
            {
                string val = value.ToString();
                Guid result = new Guid();
                if (Guid.TryParse(val, out result)) return result;
                return Guid.Empty;
            }
           
        }
        #endregion

    }
}
