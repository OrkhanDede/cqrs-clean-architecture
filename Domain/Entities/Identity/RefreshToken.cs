using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using System.Security.Claims;
using Core.Constants;
using Core.Models;
using Domain.Common.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Identity
{
    [Index("Token")]
    public class RefreshToken : Entity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime? RevokeDate { get; set; }
        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        [ForeignKey("Impersonator")]
        public string ImpersonatorId { get; set; }
        public User Impersonator { get; set; }

        [ForeignKey("User")]
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [NotMapped]
        public bool IsExpired => DateTime.Now >= ExpireDate;
        [NotMapped]
        public bool IsRevoked => RevokeDate != null;
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
        public TokenClaim ToTokenClaim()
        {


            var tokenClaim = new TokenClaim();

            if (!string.IsNullOrEmpty(ImpersonatorId))
                tokenClaim.ImpersonatorId = new Claim(CustomClaimTypeConstants.Impersonator, ImpersonatorId);
            if (Impersonator != null)
                tokenClaim.ImpersonatorName = new Claim(CustomClaimTypeConstants.ImpersonatorName, Impersonator.UserName);

            return tokenClaim;
        }

    }
}
