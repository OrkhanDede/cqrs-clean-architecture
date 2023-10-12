using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Core.Constants;

namespace Core.Models
{
    public class TokenClaim
    {
        public Claim ImpersonatorId { get; set; }
        public Claim ImpersonatorName { get; set; }
        public Claim RememberMe { get; set; }

        public TokenClaim()
        {

        }
        public TokenClaim(List<Claim> claims)
        {
            this.GenerateFromList(claims);
        }
        private void GenerateFromList(List<Claim> claims)
        {
            var rememberMeClaim = claims?.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.RememberMe);
            var impersonator = claims?.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.Impersonator);
            var impersonatorName = claims?.FirstOrDefault(c => c.Type == CustomClaimTypeConstants.ImpersonatorName);
            this.ImpersonatorId = impersonator;
            this.ImpersonatorName = impersonatorName;
            this.RememberMe = rememberMeClaim;
        }
        public List<Claim> ToClaimList()
        {
            return new List<Claim>()
            {
                ImpersonatorId,
                ImpersonatorName,
                RememberMe
            };
        }

    }
}
