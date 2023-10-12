using System.Collections.Generic;

namespace Core.Constants
{
    public static class CustomClaimTypeConstants
    {
        internal const string ClaimTypeNamespace = "http://schemas.microsoft.com/ws/2008/06/identity/claims";
        public const string RememberMe = ClaimTypeNamespace + "/rememberme";
        public const string Impersonator = ClaimTypeNamespace + "/impersonator";
        public const string ImpersonatorName = ClaimTypeNamespace + "/impersonatorName";

        public static List<string> Types = new()
        {
            RememberMe,
            Impersonator,
            ImpersonatorName
        };
    }
}
