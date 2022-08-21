using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Dapper;

using MISA.Core.Models;

using MISA.Core.Interfaces.Repository;

namespace MISA.Infrastructure.Repository
{
    /// <summary>
    /// Employee Database
    /// </summary>
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Methods
        /// <summary>
        /// Kiểm tra nếu mã nhân viên đã tồn tại trong hệ thống
        /// </summary>
        /// <param name="EmployeeCode">T</param>
        /// <returns></returns>
        public override bool CheckExistence(string EmployeeCode)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = "Proc_Employee_GetByCode";
                var parameter = new DynamicParameters();
                parameter.Add("EmployeeCode", EmployeeCode);
                var res = mySqlConnection.QueryFirstOrDefault(sql, param: parameter,
                    commandType: System.Data.CommandType.StoredProcedure);
                // nếu không có
                if (res == null) return false;
                return true;

            }
        }

        /// <summary>
        /// Nhập khẩu dữ liệu
        /// </summary>
        /// <param name="employees">danh sách nhân viên</param>
        /// <returns>số bản ghi được lưu thành công</returns>
        public int Import(IEnumerable<Employee> employees)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Lấy ra mã nhân viên lớn nhất trong database
        /// </summary>
        /// <returns></returns>
        public string LatestEmployeeCode()
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = "SELECT e.EmployeeCode FROM Employee e ORDER BY e.EmployeeCode DESC;";
                var res = mySqlConnection.QueryFirstOrDefault<string>(sql);
                
                return res;
            }
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex">chỉ mục trang</param>
        /// <param name="pageSize">số bản ghi trên một trang</param>
        /// <param name="employeeFilter">từ khóa tìm kiếm</param>
        /// <returns></returns>
        public object Paging(string? employeeFilter, int pageSize, int pageIndex)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                if (string.IsNullOrEmpty(employeeFilter)) employeeFilter = "";

                var sql = "Proc_PagingEmployee";
                var parameters = new DynamicParameters();
                parameters.Add("employeeFilter", employeeFilter);
                parameters.Add("pageIndex", pageIndex);
                parameters.Add("pageSize", pageSize);
                parameters.Add("totalRecords", direction: System.Data.ParameterDirection.Output);

                var res = mySqlConnection.Query<Object>(sql: sql, param: parameters,
                    commandType: System.Data.CommandType.StoredProcedure);
                int totalRecords = parameters.Get<int>("totalRecords");

                int totalPages = (totalRecords - (totalRecords / pageSize) * pageSize == 0) ? 
                    totalPages = totalRecords / pageSize: totalPages = totalRecords / pageSize + 1;

                return new
                {
                    TotalPage=totalPages,
                    TotalRecord= totalRecords,
                    Data=res
                };
            }
        }

        /// <summary>
        /// tìm kiếm nhân viên
        /// </summary>
        /// <param name="employeeFilter"></param>
        /// <returns></returns>
        public IEnumerable<Object> SearchEmployee(string employeeFilter)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                if (string.IsNullOrEmpty(employeeFilter)) employeeFilter="";

                var sql = "Proc_SearchEmployee";
                var param = new DynamicParameters();
                param.Add("searchPattern", employeeFilter);
                var res = mySqlConnection.Query<Object>(sql: sql, param: param, 
                    commandType: System.Data.CommandType.StoredProcedure);

                return res;
            }
        }

        

        #endregion

    }
}
