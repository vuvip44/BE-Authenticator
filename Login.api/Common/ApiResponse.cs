using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.api.Common
{
    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }

        public T Data { get; set; }

        public ApiResponse(int status, T data, string message = "Success")
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public ApiResponse(int status, string errorMessage)
        {
            Status = status;
            Message = errorMessage;
            Data = default;
        }
    }
}