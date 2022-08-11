using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MISA.Core.Interfaces.Services;
using MISA.Core.Interfaces.Repository;
using MISA.Core.Exceptions;

namespace MISA.Core.Services
{
    /// <summary>
    /// Base service
    /// </summary>
    /// <typeparam name="MISAEntity"></typeparam>
    public class BaseService<MISAEntity> : IBaseService<MISAEntity>
    {
        #region Properties
        
        /// _repository: dùng để truy cập database tầng infrastructure 

        protected IBaseRepository<MISAEntity> _repository;

        #endregion

        #region Constructor

        /// <summary>
        /// Truyền repository để truy cập database sau khi
        /// thực hiện xong nghiệp vụ
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(IBaseRepository<MISAEntity> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertService(MISAEntity entity)
        {
            // validate dữ liệu
            var isValid = Validate(entity);
            if (isValid)
            {
                // thực hiện thêm mới vào database
                var res = _repository.Insert(entity);
                return res;
            }
            else
            {
                // thông báo dữ liệu không hợp lệ
                return 0;
            }
            
        }

        /// <summary>
        /// Sửa
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int UpdateService(MISAEntity entity)
        {
            // validate dữ liệu
            var isValid = Validate(entity);
            if (isValid)
            {
                // thực hiện thêm mới vào database
                var res = _repository.Update(entity);
                return res;
            }
            else
            {
                // thông báo dữ liệu không hợp lệ
                return 0;
            }
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="entity">dữ liệu cần validate</param>
        /// <returns></returns>
        protected virtual bool Validate(MISAEntity entity)
        {
            return true;
        }

        /// <summary>
        /// Check nếu một thuộc tính nào đó trống
        /// </summary>
        /// <param name="entityProperty"></param>
        /// <returns></returns>
        public bool NotEmpty(string entityProperty, string propName)
        {
            if (!string.IsNullOrEmpty(entityProperty)) return true;
            else {
                // ném lỗi
                throw new MISAValidateException($"{propName} không được để trống");
            }
        }

        #endregion
    }
}
