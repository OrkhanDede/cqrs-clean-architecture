using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Application.Commands.UserCommands.CreateUser
{
    public class CreateUserRequest
    {

        [Required]
        [RegularExpression(
            RegexConstants.EmailRegex,
            ErrorMessage = "Email is not in valid format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegexConstants.UserNameRegex, ErrorMessage = "Username is not in valid format.")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "Password is not in valid format.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
        public List<string> DirectivePermissions { get; set; } = new List<string>();
    }
}
