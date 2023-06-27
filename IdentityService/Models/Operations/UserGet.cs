using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models.Operations
{
    public class UserGet
    {
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string login { get; set; }        
        [Required]
        public Role role { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }

        public UserGet(User user)
        {
            this.id = user.id;
            this.name = user.name;
            this.login = user.login;
            this.role = user.role;
            this.phone = user.phone;
            this.email = user.email;
        }
    }
}
