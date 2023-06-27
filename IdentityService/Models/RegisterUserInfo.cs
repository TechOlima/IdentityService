using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class RegisterUserInfo
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}
