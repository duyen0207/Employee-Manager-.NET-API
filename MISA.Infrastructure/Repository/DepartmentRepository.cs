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

        #endregion
    }
}
