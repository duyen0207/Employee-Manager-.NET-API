using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;

using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.Models;
using MISA.Core.Exceptions;

namespace MISA.Web05.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        #region Properties

        // _employeeRepository: dùng để get, get by id, delete
        // _employeeService: dùng để insert, update

        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        #endregion

        #region Constructor

        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        #endregion


        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns>danh sách nhân viên</returns>
        [HttpGet]
        public IActionResult? Get()
        {
            try
            {
                var employees = _employeeRepository.Get();
                return Ok(employees);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }
        }

        /// <summary>
        /// Lấy thông tin nhân viên theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>nhân viên cụ thể</returns>
        [HttpGet("{employeeId}")]
        public IActionResult? Get(string employeeId)
        {
            try
            {
                var employee = _employeeRepository.Get(employeeId);
                // Trả dữ liệu cho client
                return Ok(employee);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }
        }

        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns></returns>
        [HttpGet("NewEmployeeCode")]
        public IActionResult? GetNewCode()
        {
            try
            {
                string newCode = _employeeService.GetNewCode();
                return Ok(newCode);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        [HttpPost]
        public IActionResult? Post(Employee employee)
        {
            try
            {
                var res = _employeeService.InsertService(employee);
                return StatusCode(201, res);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }
        }

        /// <summary>
        /// Sửa
        /// </summary>
        [HttpPut("{employeeId}")]
        public IActionResult? Put(Guid employeeId, Employee updateEmployee)
        {
            try
            {
                updateEmployee.EmployeeId = employeeId;

                var res = _employeeService.UpdateService(updateEmployee);
                return Ok(res);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }
        }

        /// <summary>
        /// Xóa
        /// </summary>
        [HttpDelete("{employeeId}")]
        public IActionResult? Delete(Guid employeeId)
        {
            try
            {
                var res = _employeeRepository.Delete(employeeId);
                return Ok(res);
            }
            catch (Exception exception)
            {
                return this.HandleException(exception);
            }   
        }

        /// <summary>
        /// Xử lý lỗi 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private IActionResult HandleException(Exception exception)
        {
            // ghi log vào hệ thống
            //...

            // thông báo lỗi
            string userMsg = null;
            int statusCode = 200;

            if (exception is MISAValidateException)
            {
                statusCode = 400;
                // nếu là lỗi validate dữ liệu thì thông báo cho người dùng biết
                userMsg = exception.Message;
            }
            else
            {
                userMsg = "Có lỗi xảy ra vui lòng liên hệ MISA để được trợ giúp";
                statusCode = 500;
            }

            var res = new
            {
                devMsg = exception.Message,
                userMsg = userMsg
            };
            return StatusCode(statusCode, res);

        }

    }
}
