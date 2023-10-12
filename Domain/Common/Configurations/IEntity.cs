using System;
using Core.Enums;
using Domain.Entities.Identity;

namespace Domain.Common.Configurations
{
    public interface IEntity
    {
        RecordStatusEnum Status { get; set; }
        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        DateTime? DateDeleted { get; set; }
        string Uuid { get; set; }
        User CreatedBy { get; set; }
        string CreatedById { get; set; }
        User ModifiedBy { get; set; }
        string ModifiedById { get; set; }
    }
}
