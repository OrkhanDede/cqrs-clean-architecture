namespace Core.Constants
{
    public class RegexConstants
    {

        public const string UserNameRegex = @"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{4,32}$";

        public const string EmailRegex = @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$";

        //public string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#$^+=!*()@%&]).{8,100}$";
        public const string PasswordRegex = @"^.{5,100}$";

    }
}
