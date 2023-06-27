using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class Role
    {
        public int id { get; set; }
        public string name { get; set; }        

        [Display(Description = "Описание")]
        public string description { get; set; }

        [Display(Description = "Уровни авторизации")]
        public string authority { get; set; }

        [Display(Description = "Полномочия роли")]
        public ICollection<RoleScope> scopes { get; set; }
    }
}
