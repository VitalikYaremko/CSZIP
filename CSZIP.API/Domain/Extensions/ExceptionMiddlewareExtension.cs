using CSZIP.API.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static IApplicationBuilder HandleErrors(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
