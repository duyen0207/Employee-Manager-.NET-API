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
    /// Department Database
    /// </summary>
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        #region Methods

        /// <summary>
        /// Kiểm tra trùng lặp tên phòng ban
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        public override bool CheckExistence(string departmentName)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"SELECT * FROM Department WHERE DepartmentName={departmentName}";
                var res = mySqlConnection.QueryFirstOrDefault(sql);
                return res;

            }
        }

        /// <summary>
        /// tìm id của department theo tên
        /// </summary>
        /// <param name="departmentName">tên phòng ban</param>
        /// <returns>id của phòng ban</returns>
        public object GetId(string departmentName)
        {
            if (string.IsNullOrEmpty(departmentName)) return null;
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"SELECT DepartmentId FROM Department WHERE DepartmentName='{departmentName}'";
                var res = mySqlConnection.QueryFirstOrDefault(sql);
                
                return res.DepartmentId;

            }
        }

        #endregion
    }
}
