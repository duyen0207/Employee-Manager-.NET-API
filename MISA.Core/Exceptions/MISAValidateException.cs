using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Core.Exceptions
{
    /// <summary>
    /// Exception khi validate dữ liệu
    /// </summary>
    public class MISAValidateException : Exception
    {
        #region Properties
        /// <summary>
        /// Lời nhắn thông báo lỗi validate
        /// </summary>
        public string? ValidateErrorMsg { get; set; }

        public override string Message => this.ValidateErrorMsg;
        #endregion

        #region Constructor
        public MISAValidateException(string? validateErrorMsg)
        {
            ValidateErrorMsg = validateErrorMsg;
        }

        #endregion
        
    }
}
