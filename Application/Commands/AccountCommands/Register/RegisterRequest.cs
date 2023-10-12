using System.ComponentModel.DataAnnotations;
using Core.Constants;

namespace Application.Commands.AccountCommands.Register
{
    public class RegisterRequest
    {
        [Required]
        [RegularExpression(
            RegexConstants.EmailRegex,
            ErrorMessage = "Email is not in valid format.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(RegexConstants.UserNameRegex, ErrorMessage = "Username is not in valid format.")]
        public string UserName { get; set; }
        //[Required]
        //public string Name { get; set; }
        //[Required]
        //public string Surname { get; set; }

        [Required]
        [RegularExpression(RegexConstants.PasswordRegex, ErrorMessage = "Password is not in valid format.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
