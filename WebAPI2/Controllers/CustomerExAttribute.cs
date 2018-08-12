using System;
using System.Web.Http.Filters;
using System.Net.Http;
using WebAPI2.Models;
using System.Web.Http;

namespace WebAPI2.Controllers
{
    /// <summary>
    /// 客製化錯誤訊息Filter
    /// </summary>
    public class CustomerExAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.InternalServerError,
                Content = new ObjectContent<CustomerVM>(new CustomerVM {
                    Error_Code = 99,
                    Error_Msg = actionExecutedContext.Exception.Message
                }, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }

    }
}