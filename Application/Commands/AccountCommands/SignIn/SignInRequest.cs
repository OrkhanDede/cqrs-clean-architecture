using System.ComponentModel.DataAnnotations;

namespace Application.Commands.AccountCommands.SignIn
{
    public class SignInRequest
    {
        [Required(ErrorMessage = "Email or Username is not defined")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage = "Password is not defined")]
        public string Password { get; set; }

    }
}
