using Microsoft.AspNetCore.Http;
using System;

namespace WebParser.Api.Common
{
    /// <summary>
    /// Common exception for scanning
    /// </summary>
    public class ScanException : Exception
    {
        public int StatusCode { get; set; }
        public ScanException()
        {
        }
        public ScanException(string message, int statusCode = StatusCodes.Status500InternalServerError) : base(String.Format("Error : {0}", message))
        {
            StatusCode = statusCode;
        }
    }
}
