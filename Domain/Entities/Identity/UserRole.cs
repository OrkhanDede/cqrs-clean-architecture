using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Domain.Common.Configurations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class UserRole : IdentityUserRole<string>, IEntity
    {

        public User User { get; set; }
        public Role Role { get; set; }
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string Uuid { get; set; }
        public User CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public string CreatedById { get; set; }
        public User ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public string ModifiedById { get; set; }
    }

}
