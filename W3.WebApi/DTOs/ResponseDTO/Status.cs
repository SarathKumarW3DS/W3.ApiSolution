using Microsoft.AspNetCore.Mvc;

namespace W3.WebApi.DTOs.ResponseDTO
{
    public class Status
    {
        public Status(StatusCodeResult statusCodeResult)
        {
            StatusCodeResult = statusCodeResult;
        }

        public object data { get; set; }
        public string Message { get; set; }
        public bool success { get; set; }
        public StatusCodeResult StatusCodeResult { get; }
    }
}
