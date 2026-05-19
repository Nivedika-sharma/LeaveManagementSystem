using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class OperationResult
    {
        public int Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }

        public OperationResult()
        {
        }

        public OperationResult(int status, string message, object? data = null)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
