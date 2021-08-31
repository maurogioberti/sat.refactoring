using System;

namespace Sat.Recruitment.ResourceAccess.Responses
{
    public class ValidationResponse
    {
        public ValidationResponse(bool sucess, string message)
        {
            this.Success = sucess;
            this.Message = message;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}