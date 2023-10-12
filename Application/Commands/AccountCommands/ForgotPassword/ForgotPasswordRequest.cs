using System.ComponentModel.DataAnnotations;
using Core.Constants;
using Core.Resources;

namespace Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = ResourceKey.Required)]
        public string EmailOrUsername { get; set; }
        [Required(ErrorMessage = ResourceKey.Required)]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = ResourceKey.DataIsNotValidFormat)]
        public string Password { get; set; }
        public string Code { get; set; }
    }
}
