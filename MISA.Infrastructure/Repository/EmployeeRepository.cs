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
        /// <param name="EmployeeCode"></param>
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
                return res;

            }
        }

        /// <summary>
        /// Lấy ra bản ghi mới nhất vừa được thêm vào
        /// </summary>
        /// <returns></returns>
        public Employee LatestEmployee()
        {
            var sql = "SELECT e.CreatedDate, e.FullName FROM Employee e ORDER BY e.CreatedDate DESC;";
            var res = mySqlConnection.QueryFirstOrDefault(sql);

            return res;
        }

        #endregion

    }
}
