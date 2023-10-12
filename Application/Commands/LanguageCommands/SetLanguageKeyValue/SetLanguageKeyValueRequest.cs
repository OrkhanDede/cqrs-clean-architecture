using System.ComponentModel.DataAnnotations;

namespace Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public class SetLanguageKeyValueRequest
    {
        [Required]
        public string LanguageId { get; set; }
        [Required]
        public int KeyId { get; set; }
        public string Value { get; set; }
    }
}
