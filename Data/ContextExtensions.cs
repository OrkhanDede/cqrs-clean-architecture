using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common.Configurations;
using Domain.Entities.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data
{
    public static class ContextExtensions
    {
        public static ApplicationDbContext DetectChanges(this ApplicationDbContext context, List<EntityEntry> changes, params string[] exceptTables)
        {
            var auditEntries = new List<AuditEntry>();
            if (exceptTables != null && exceptTables.Any())
                changes = changes.Where(c =>
                    !exceptTables.Contains(c.Entity.GetType().Name, StringComparer.InvariantCultureIgnoreCase)).ToList();

            foreach (var entry in changes)
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;

                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (propertyName == nameof(Entity.Uuid) && property.CurrentValue != null)
                    {
                        if (!string.IsNullOrEmpty(property.CurrentValue.ToString()))
                            auditEntry.Uuid = property.CurrentValue.ToString();

                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns[propertyName] = property.CurrentValue;
                            }
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                context.AuditLogs.Add(auditEntry.ToAudit());
            }

            return context;

        }
    }
}
