using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Application.Commands.UserCommands.ChangeUserPassword
{
    public class ChangeUserPasswordRequest
    {


        [Required]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = "Old Password is not in valid format.")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(RegexConstants.PasswordRegex,
            ErrorMessage = "Password is not in valid format.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
