using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class RoleScope
    {
        [Display(Description = "ид записи")]
        public int id { get; set; }

        [Display(Description = "ИД роли. ")]
        public int roleId { get; set; }
        [Display(Description = "Роль. ")]
        public Role role { get; set; }

        [Display(Description = "ИД полномочия. ")]
        public int scopeId { get; set; }
        [Display(Description = "Полномочие. ")]
        public Scope scope { get; set; }
    }
}
