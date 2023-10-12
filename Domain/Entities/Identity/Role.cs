using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Domain.Common.Configurations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class Role : IdentityRole<string>, IEntity
    {
        public ICollection<UserRole> Users { get; set; } = new Collection<UserRole>();
        public ICollection<RolePermissionCategory> PermissionCategory { get; set; } = new Collection<RolePermissionCategory>();
        [StringLength(256)]
        public string Description { get; set; }
        // public bool IsEditable { get; set; } = true;
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string Uuid { get; set; }
        public RecordStatusEnum Status { get; set; }
        public User CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        public User ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string ModifiedById { get; set; }

    }
}
