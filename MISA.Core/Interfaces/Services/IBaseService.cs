using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Interfaces.Services
{
    /// <summary>
    /// Dùng để thực hiện nghiệp vụ của hệ thống
    /// </summary>
    /// ví dụ: validate dữ liệu trước khi thêm, sửa,...
    public interface IBaseService<MISAEntity>
    {

        #region Methods
        int InsertService(MISAEntity entity);

        int UpdateService(MISAEntity entity);
        #endregion

    }
}
