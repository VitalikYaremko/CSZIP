using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Domain.Models
{
    public class ErrorModel
    {
        public ErrorModel(Exception exception)
        {
            ErrorMessage = exception.Message;
            StackTrace = exception.StackTrace;
        }

        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
