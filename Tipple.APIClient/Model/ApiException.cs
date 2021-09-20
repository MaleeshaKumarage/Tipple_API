using System;
using System.Collections.Generic;
using System.Text;

namespace Tipple.APIClient.Model
{
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }
        public CommonError ErrorContent { get; set; }
        public ApiException() { }
        public ApiException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
        public ApiException(int errorCode, string message, CommonError errorContent = null) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorContent = errorContent;
        }
        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
