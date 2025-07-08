using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceTracker.Core.DTOs
{
    public class OperationResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public OperationResultDto(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
