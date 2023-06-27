using IdentityService.Models.Operations;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class User
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string login { get; set; }
        [Required]
        public string password { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public int roleid { get; set; }
        public Role role { get; set; }        
    }
}
