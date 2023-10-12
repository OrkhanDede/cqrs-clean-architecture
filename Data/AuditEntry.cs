using System;
using System.Collections.Generic;
using Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Data
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        // public string UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public Dictionary<string, object> ChangedColumns { get; } = new Dictionary<string, object>();
        public string Uuid { get; set; }
        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            // audit.UserId = UserId;
            audit.AuditType = AuditType;
            audit.TableName = TableName;
            audit.DateTime = DateTime.UtcNow;
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.Uuid = Uuid;
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return audit;
        }
    }

}
