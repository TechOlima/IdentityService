using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Operations
{
    public class UserPut
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string login { get; set; }        
        [Required]
        public string role { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}
