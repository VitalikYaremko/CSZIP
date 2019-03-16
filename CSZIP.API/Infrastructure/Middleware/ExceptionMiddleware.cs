using CSZIP.API.Domain.Interfaces.Services;
using CSZIP.API.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CSZIP.API.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOutputFormatter _outputFormatter;
        private readonly IHttpResponseStreamWriterFactory _streamWriterFactory;


        public ExceptionMiddleware(RequestDelegate next, JsonOutputFormatter outputFormatter, IHttpResponseStreamWriterFactory streamWriterFactory)
        {
            _next = next;
            _outputFormatter = outputFormatter;
            _streamWriterFactory = streamWriterFactory;
        }

        public async Task InvokeAsync(HttpContext context, ILogger logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception.ToString());
                var errorModel = new ErrorModel(exception);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await _outputFormatter.WriteAsync(new OutputFormatterWriteContext(context, _streamWriterFactory.CreateWriter, typeof(ErrorModel), errorModel));
            }
        }
    }
}
