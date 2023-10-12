using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Configurations;

namespace Domain.Entities.Lang
{
    public class LanguageKey : Entity
    {
        [Key]
        [ForeignKey("Language")]
        public string LanguageId { get; set; }
        [Key]
        [ForeignKey("Key")]
        public int KeyId { get; set; }


        public Language Language { get; set; }
        public Key Key { get; set; }
        public string Value { get; set; }


    }
}
