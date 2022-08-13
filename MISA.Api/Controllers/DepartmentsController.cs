using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using MySqlConnector;

using MISA.Core.Interfaces.Repository;
using MISA.Core.Interfaces.Services;
using MISA.Core.Models;

namespace MISA.Web05.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        #region Properties

        // _departmentRepository: dùng để get, get by id, delete
        // _departmentService: dùng để insert, update

        IDepartmentRepository _departmentRepository;
        IDepartmentService _departmentService;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService)
        {
            _departmentRepository = departmentRepository;
            _departmentService = departmentService;
        }
        #endregion

        #region Methods

        
        /// <summary>
        /// Lấy danh sách phòng ban
        /// </summary>
        /// <returns>danh sách phòng ban</returns>
        [HttpGet]
        public IActionResult? Get()
        {
            try
            {
                var departments = _departmentRepository.Get();
                return Ok(departments);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin phòng ban theo id
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns>phòng ban cụ thể</returns>
        [HttpGet("{departmentId}")]
        public IActionResult? Get(string departmentId)
        {
            try
            {
                var department = _departmentRepository.Get(departmentId);
                return Ok(department);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        [HttpPost]
        public IActionResult? Post(Department department)
        {
            try
            {
                var res = _departmentService.InsertService(department);
                return StatusCode(201, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Sửa
        /// </summary>
        [HttpPut("{departmentId}")]
        public IActionResult? Put(Guid departmentId, Department updateDepartment)
        {
            try
            {
                var res = _departmentService.UpdateService(updateDepartment);
                return Ok(res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Xóa
        /// </summary>
        [HttpDelete("{departmentId}")]
        public IActionResult? Delete(Guid departmentId)
        {
            try
            {
                var res = _departmentRepository.Delete(departmentId);
                return Ok(res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion


    }
}
