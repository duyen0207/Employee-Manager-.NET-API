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

        #endregion

    }
}
