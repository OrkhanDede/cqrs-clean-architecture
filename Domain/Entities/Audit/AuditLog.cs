using System;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;

namespace Domain.Entities.Audit
{

    public class AuditLog
    {
        public Guid Id { get; set; }

        [ForeignKey("CreatedBy")]
        // [Required]
        public string CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        [Column(TypeName = "json")]
        public string OldValues { get; set; }
        [Column(TypeName = "json")]
        public string NewValues { get; set; }
        public string Uuid { get; set; }
        public string AffectedColumns { get; set; }
        public AuditType AuditType { get; set; }
    }
    public enum AuditType
    {
        Create = 1,
        Update,
        Delete
    }
}
