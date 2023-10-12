using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Domain.Common.Configurations;

namespace Domain.Entities.Identity
{
    public class PermissionCategory : Entity
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Label { get; set; }
        [StringLength(128)]
        public string VisibleLabel { get; set; }
        [StringLength(256)]
        public string Description { get; set; }

        public ICollection<PermissionCategoryPermission> PossiblePermissions { get; set; } = new Collection<PermissionCategoryPermission>();

    }
}
