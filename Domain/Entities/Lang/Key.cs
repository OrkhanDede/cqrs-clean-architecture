using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Configurations;

namespace Domain.Entities.Lang
{
    public class Key : Entity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        public string Label { get; set; }

        [ForeignKey("Parent")]
        public int? ParentId { get; set; }

        public Key Parent { get; set; }
        public ICollection<Key> Children { get; set; } = new Collection<Key>();
        public ICollection<LanguageKey> Languages { get; set; } = new List<LanguageKey>();

        [NotMapped]
        public bool IsRoot => Parent != null;
    }
}
