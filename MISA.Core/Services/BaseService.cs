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
        protected List<string> ValidateErrorMsgs;
        protected bool IsValid;

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
            ValidateErrorMsgs = new List<string>();
            IsValid = true;
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
            var isValid = this.Validate(entity);
            if (isValid)
            {
                // thực hiện thêm mới vào database
                var res = _repository.Insert(entity);
                return res;
            }
            else
            {
                
                // thông báo dữ liệu không hợp lệ
                throw new MISAValidateException(String.Join(", ", ValidateErrorMsgs));

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
            var isValid = this.Validate(entity);
            if (isValid)
            {
                // thực hiện thêm mới vào database
                var res = _repository.Update(entity);
                return res;
            }
            else
            {
                // thông báo dữ liệu không hợp lệ
                throw new MISAValidateException(ValidateErrorMsgs.ToString());
                
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
        /// Đặt tất cả các giá trị validate về mặc định
        /// </summary>
        protected void ResetValidate()
        {
            IsValid = true;
            ValidateErrorMsgs.Clear();
        }

        /// <summary>
        /// Check nếu một thuộc tính nào đó trống
        /// </summary>
        /// <param name="entityProperty"></param>
        /// <returns>true: empty, false: not empty</returns>
        public bool CheckEmpty(string entityProperty, string propName)
        {
            if (string.IsNullOrEmpty(entityProperty))
            {
                IsValid=false;
                ValidateErrorMsgs.Add($"{propName} không được để trống");
                return true;

            }
            else return false;
        }

        #endregion
    }
}
