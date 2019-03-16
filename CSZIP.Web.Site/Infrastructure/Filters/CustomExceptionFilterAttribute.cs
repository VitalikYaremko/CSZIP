using CSZIP.Web.Site.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.Web.Site.Infrastructure.Filters
{
    public class CustomExceptionFilterAttribute : TypeFilterAttribute
    {
        public CustomExceptionFilterAttribute(): base(typeof(RequiresCustomExceptionFilterAttribute))
        {

        }

        public class RequiresCustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
        {
            private readonly ILogger _logger;
            public RequiresCustomExceptionFilterAttribute(ILogger logger)
            {
                _logger = logger;
            }
            public override void OnException(ExceptionContext filterContext)
            {

                string actionName = filterContext.ActionDescriptor.DisplayName;
                string exceptionStack = filterContext.Exception.StackTrace;
                string exceptionMessage = filterContext.Exception.Message;

                var httpException = filterContext.Exception;

                string view = "Error500";
                if (httpException != null)
                {
                    if (httpException.HResult == 404)
                    {
                        view = "Error404";
                    }
                }

                filterContext.Result = new ViewResult
                {
                    ViewName = view
                };

                if (httpException.HResult > 0)
                {
                    filterContext.HttpContext.Response.StatusCode = httpException.HResult;
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = 500;
                }

                filterContext.ExceptionHandled = true;

                _logger.LogError($"In method {actionName} an exception occured: \n {exceptionMessage} \n {exceptionStack}");
            }
        }

    } 
}
