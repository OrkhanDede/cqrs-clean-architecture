using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Core.Constants;
using Core.Enums;
using Data.Configurations;
using Domain.Entities;
using Domain.Entities.Audit;
using Domain.Entities.Identity;
using Domain.Entities.Lang;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities.App.AppDomain;
namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<User,
        Role, string, UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>

    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region Identity
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<PermissionCategory> PermissionCategories { get; set; }
        public DbSet<PermissionCategoryPermission> PermissionCategoryPermissions { get; set; }
        public DbSet<RolePermissionCategory> RolePermissionCategories { get; set; }
        public DbSet<AccessLog> AccessLogs { get; set; }
        #endregion
        #region Language
        public DbSet<Language> Languages { get; set; }
        public DbSet<Key> Keys { get; set; }
        public DbSet<LanguageKey> LanguageKeys { get; set; }


        #endregion
        public DbSet<AppDomain> AppDomains { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<FileStore> FileStores { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RolePermissionCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserPermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageKeyConfiguration());

            modelBuilder.Entity<User>().ToTable("Users", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<Role>().ToTable("Roles", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserClaim>().ToTable("UserClaims", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserLogin>().ToTable("UserLogins", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserRole>().ToTable("UserRoles", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserToken>().ToTable("UserTokens", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<RolePermissionCategory>().ToTable("RolePermissionCategories", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<UserPermission>().ToTable("UserPermissions", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<Permission>().ToTable("Permissions", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<PermissionCategory>().ToTable("PermissionCategories", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<PermissionCategoryPermission>().ToTable("PermissionCategoryPermissions", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<EmailConfirmationRequest>().ToTable("EmailConfirmationRequests", SchemaNames.IdentityTableSchemaName);
            modelBuilder.Entity<PasswordResetRequest>().ToTable("PasswordResetRequests", SchemaNames.IdentityTableSchemaName);

            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs", SchemaNames.LogTableSchemaName);
            modelBuilder.Entity<AccessLog>().ToTable("AccessLogs", SchemaNames.LogTableSchemaName);

            modelBuilder.Entity<Language>().ToTable("Languages", SchemaNames.LanguageTableSchemaName);
            modelBuilder.Entity<Key>().ToTable("Keys", SchemaNames.LanguageTableSchemaName);
            modelBuilder.Entity<LanguageKey>().ToTable("LanguageKeys", SchemaNames.LanguageTableSchemaName);

            modelBuilder.SetStatusQueryFilter();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();
            var changes = ChangeTracker.Entries();
            this.DetectChanges(changes.ToList(), "AccessLog");
            OnBeforeSaving();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            var changes = ChangeTracker.Entries();
            this.DetectChanges(changes.ToList(), "AccessLog");
            OnBeforeSaving();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entry in ChangeTracker.Entries())
            {
                var propertyNames = entry.Properties.Select(c => c.Metadata.Name).ToList();

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (propertyNames.Any(c => c == "DateCreated"))
                            entry.CurrentValues["DateCreated"] = DateTime.Now;

                        if (propertyNames.Any(c => c == "CreatedById"))
                            entry.CurrentValues["CreatedById"] = userId;
                        break;
                    case EntityState.Modified:
                        if (propertyNames.Any(c => c == "DateModified"))
                            entry.CurrentValues["DateModified"] = DateTime.Now;

                        if (propertyNames.Any(c => c == "ModifiedById"))
                            entry.CurrentValues["ModifiedById"] = userId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        if (propertyNames.Any(c => c == "DateDeleted"))
                            entry.CurrentValues["DateDeleted"] = DateTime.Now;
                        if (propertyNames.Any(c => c == "Status"))
                            entry.CurrentValues["Status"] = RecordStatusEnum.Deleted;
                        break;
                }
            }

        }
    }

}
