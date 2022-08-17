using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySqlConnector;

using MISA.Core.Interfaces.Repository;

namespace MISA.Infrastructure.Repository
{
    /// <summary>
    /// Truy cập thêm, sửa, xóa dữ liệu
    /// </summary>
    /// <typeparam name="MISAEntity"></typeparam>
    public class BaseRepository<MISAEntity> : IBaseRepository<MISAEntity>
    {
        #region Properties
        /// connectionString: lưu trữ thông tin kết nối database
        /// sqlConnection: tạo kết nối với database
        /// TableName: tên bảng database cần truy cập 

        protected string connectionString;
        protected MySqlConnection mySqlConnection;
        protected string TableName;
        #endregion

        #region Constructor
        public BaseRepository()
        {
            connectionString = "Host=3.0.89.182;" +
                    "Port=3306; " +
                    "Database=MISA.WEB05.LTBDUYEN; " +
                    "User Id = dev; " +
                    "Password=12345678";
            TableName = typeof(MISAEntity).Name;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Kiểm tra sự tồn tại của entity trong database
        /// </summary>
        /// <param name="entityProp"></param>
        /// <returns></returns>
        public virtual bool CheckExistence(string entityProp)
        {
            Console.WriteLine("base repository");
            return true;
        }

        /// <summary>
        /// Get toàn bộ bảng dữ liệu
        /// </summary>
        /// <returns>MISA entity</returns>
        public IEnumerable<MISAEntity> Get()
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"SELECT * FROM {TableName}";
                var data = mySqlConnection.Query<MISAEntity>(sql);
                return data;
            }
        }

        /// <summary>
        /// Get dữ liệu theo id của entity
        /// </summary>
        /// <param name="id">id của entity</param>
        /// <returns>entity đầu tiên tìm được</returns>
        public MISAEntity Get(string id)
        {
            using(mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"SELECT * FROM {TableName} WHERE {TableName}Id = @{TableName}Id";
                var parameters = new DynamicParameters();
                parameters.Add($"{TableName}Id", id);

                var data = mySqlConnection.QueryFirstOrDefault<MISAEntity>(sql: sql, param:parameters);
                return data;
            }
        }

        /// <summary>
        /// Thêm mới bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(MISAEntity entity)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"Proc_Insert{TableName}";
                var res = mySqlConnection.Execute(sql, param: entity, 
                    commandType: System.Data.CommandType.StoredProcedure);
                return res;

            }
        }

        /// <summary>
        /// Sửa đổi một bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Update(MISAEntity entity)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"Proc_Edit{TableName}";
                var res = mySqlConnection.Execute(sql, param: entity,
                    commandType: System.Data.CommandType.StoredProcedure);
                return res;

            }
        }

        /// <summary>
        /// Xóa bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int Delete(Guid id)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"Proc_Delete{TableName}";
                var parameter = new DynamicParameters();
                parameter.Add($"{TableName}Id", id);
                var res = mySqlConnection.Execute(sql, param: parameter,
                    commandType: System.Data.CommandType.StoredProcedure);
                return res;

            }

        }

        /// <summary>
        /// Xóa hàng loạt
        /// </summary>
        /// <param name="IdList">danh sách id cần xóa</param>
        /// <returns></returns>
        public int DeleteMultiple(string IdList)
        {
            using (mySqlConnection = new MySqlConnection(connectionString))
            {
                var sql = $"DELETE FROM {TableName} WHERE {TableName}Id IN({IdList});";
                var res = mySqlConnection.Execute(sql: sql, commandType: System.Data.CommandType.Text);
                return res;

            }
        }

        #endregion

    }
}
