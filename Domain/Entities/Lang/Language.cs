using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Configurations;

namespace Domain.Entities.Lang
{
    public class Language : Entity
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Code { get; set; }
        [Required]
        [StringLength(32)]
        public string Title { get; set; }

        public ICollection<LanguageKey> LanguageKeys { get; set; } = new Collection<LanguageKey>();
    }
}
