using System;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Enums;
using Domain.Entities.Identity;

namespace Domain.Common.Configurations
{
    public class Entity : IEntity
    {
        public RecordStatusEnum Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }
        public string Uuid { get; set; }
        public virtual User CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual string CreatedById { get; set; }

        public virtual User ModifiedBy { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual string ModifiedById { get; set; }

        public Entity()
        {
            Status = RecordStatusEnum.Active;
            Uuid = Guid.NewGuid().ToString();
        }
    }
}
