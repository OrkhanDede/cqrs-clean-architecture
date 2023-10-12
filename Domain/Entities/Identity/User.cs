using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Domain.Common.Configurations;
using Domain.Entities.Audit;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{

    public class User : IdentityUser<string>, IEntity
    {


        public string Name { get; set; }
        public string SurName { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string Uuid { get; set; }
        public DateTime? LastAccessFailedAttempt { get; set; }

        [InverseProperty("User")]
        public ICollection<UserRole> Roles { get; set; } = new Collection<UserRole>();
        [InverseProperty("CreatedBy")]
        public ICollection<UserRole> CreatedRoles { get; set; } = new Collection<UserRole>();
        [InverseProperty("ModifiedBy")]
        public ICollection<UserRole> ModifiedRoles { get; set; } = new Collection<UserRole>();


        [InverseProperty("User")]
        public ICollection<UserPermission> DirectivePermissions { get; set; } = new Collection<UserPermission>();
        [InverseProperty("CreatedBy")]
        public ICollection<UserPermission> CreatedDirectivePermissions { get; set; } = new Collection<UserPermission>();
        [InverseProperty("ModifiedBy")]
        public ICollection<UserPermission> ModifiedDirectivePermissions { get; set; } = new Collection<UserPermission>();



        [InverseProperty("User")]
        public ICollection<EmailConfirmationRequest> EmailConfirmationRequests { get; set; } = new Collection<EmailConfirmationRequest>();
        [InverseProperty("CreatedBy")]
        public ICollection<EmailConfirmationRequest> CreatedEmailConfirmationRequests { get; set; } = new Collection<EmailConfirmationRequest>();
        [InverseProperty("ModifiedBy")]
        public ICollection<EmailConfirmationRequest> ModifiedEmailConfirmationRequests { get; set; } = new Collection<EmailConfirmationRequest>();


        [InverseProperty("User")]
        public ICollection<PasswordResetRequest> PasswordResetRequests { get; set; } = new Collection<PasswordResetRequest>();
        [InverseProperty("CreatedBy")]
        public ICollection<PasswordResetRequest> CreatedPasswordResetRequests { get; set; } = new Collection<PasswordResetRequest>();
        [InverseProperty("ModifiedBy")]
        public ICollection<PasswordResetRequest> ModifiedPasswordResetRequests { get; set; } = new Collection<PasswordResetRequest>();


        public ICollection<AccessLog> Logs { get; set; } = new Collection<AccessLog>();
        public ICollection<AuditLog> AuditLogs { get; set; } = new Collection<AuditLog>();


        [InverseProperty("CreatedBy")]
        public ICollection<User> CreatedUsers { get; set; } = new Collection<User>();
        [InverseProperty("ModifiedBy")]
        public ICollection<User> ModifiedUsers { get; set; } = new Collection<User>();

        [InverseProperty("User")]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new Collection<RefreshToken>();
        [InverseProperty("CreatedBy")]
        public ICollection<RefreshToken> CreatedRefreshTokens { get; set; } = new Collection<RefreshToken>();
        [InverseProperty("ModifiedBy")]
        public ICollection<RefreshToken> ModifiedRefreshTokens { get; set; } = new Collection<RefreshToken>();

        public User CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }

        public User ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string ModifiedById { get; set; }
        public RecordStatusEnum Status { get; set; }
    }
}
