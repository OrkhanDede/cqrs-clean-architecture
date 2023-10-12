using System;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Configurations;

namespace Domain.Entities.Identity
{
    public class EmailConfirmationRequest : Entity
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
