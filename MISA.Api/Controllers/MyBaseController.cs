using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using MISA.Core.Exceptions;


namespace MISA.Api.Controllers
{
    public class MyBaseController : ControllerBase
    {
        /// <summary>
        /// Xử lý lỗi 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected IActionResult HandleException(Exception exception)
        {
            // ghi log vào hệ thống
            //...

            // thông báo lỗi
            string userMsg = null;
            int statusCode = 200;

            if (exception is MISAValidateException)
            {
                statusCode = 400;
                // nếu là lỗi validate dữ liệu thì thông báo cho người dùng biết
                userMsg = exception.Message;
            } else {
                userMsg = "Có lỗi xảy ra vui lòng liên hệ MISA để được trợ giúp";
                statusCode = 500;
            }
            
            var res = new
            {
                devMsg = exception.Message,
                userMsg = userMsg
            };
            return StatusCode(statusCode, res);
            
        }
    }
}
