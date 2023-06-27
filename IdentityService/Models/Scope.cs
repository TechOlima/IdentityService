using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    /// <summary>
    /// Полномочия.
    /// </summary>
    public class Scope
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [Display(Description = "ид записи")]
        public int id { get; set; }

        [Display(Description = "Наименование полномочия")]
        public string name { get; set; }

        [Display(Description = "Описание")]
        public string description { get; set; }
    }
}
