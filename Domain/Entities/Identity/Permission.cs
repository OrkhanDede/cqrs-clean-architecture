using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Configurations;

namespace Domain.Entities.Identity
{
    public class Permission : Entity
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Label { get; set; }

        [StringLength(32)]
        public string VisibleLabel { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
        public bool IsDirective { get; set; }

        public ICollection<PermissionCategoryPermission> Categories { get; set; }

        public ICollection<UserPermission> Users { get; set; } = new Collection<UserPermission>();
        public Permission() { Categories = new Collection<PermissionCategoryPermission>(); }
    }
}
