using System.ComponentModel.DataAnnotations;

namespace W3.WebApi.DTOs.RequestDTO
{
    public class RegisterDTO
    {
        public string userName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        [StringLength(10, MinimumLength = 6)]
        public string mobile { get; set; }
        [StringLength(10, MinimumLength = 6)]
        public string alternateMobile { get; set; }
        public string address { get; set; }
    }
}
