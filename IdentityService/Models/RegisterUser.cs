using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Поле name должно быть заполнено")]
        public string name { get; set; }

        [Required(ErrorMessage = "Поле login должно быть заполнено")]
        public string login { get; set; }

        [Required(ErrorMessage = "Поле password должно быть заполнено")]
        public string password { get; set; }

        [Required(ErrorMessage = "Поле passwordconfirm должно быть заполнено")]
        [Compare("password", ErrorMessage = "Поле password и passwordconfirm должны совпадать")]
        public string passwordconfirm { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
    }
}
