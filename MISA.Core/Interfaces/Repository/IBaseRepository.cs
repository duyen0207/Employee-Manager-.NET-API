using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Repository
{
    /// <summary>
    /// Interface chung cho thêm, sửa, xóa database
    /// </summary>
    /// <typeparam name="MISAEntity"></typeparam>
    /// Created by: LTB Duyen (09/8/2022)
    
    public interface IBaseRepository<MISAEntity>
    {
        #region Methods
        /// <summary>
        /// Kiểm tra sự trùng lặp dữ liệu
        /// </summary>
        /// <param name="entityProp"></param>
        /// <returns></returns>
        bool CheckExistence(string entityProp);

        /// <summary>
        /// Lấy danh sách dữ liệu
        /// </summary>
        /// <returns></returns>
        IEnumerable<MISAEntity> Get();

        /// <summary>
        /// Lấy dữ liệu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MISAEntity Get(string id);

        /// <summary>
        /// Thêm mới một hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(MISAEntity entity);

        /// <summary>
        /// Sửa 1 hàng
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(MISAEntity entity);

        /// <summary>
        /// Xóa 1 hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(Guid id);

        /// <summary>
        /// Xóa hàng loạt
        /// </summary>
        /// <param name="IdList">danh sách id cần xóa</param>
        /// <returns></returns>
        int DeleteMultiple(string IdList);
        #endregion
    }
}
