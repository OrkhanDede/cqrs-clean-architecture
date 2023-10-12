namespace Application.Commands.AccountCommands.SignIn
{
    public class SignInResponse
    {
        public bool IsSigned { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
