using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ticketManage.Models;
using ticketManage.Services;
using ticketManage.Utils;

namespace ticketManage.Data
{
    public class AuditSaveChangesInterceptor(CurrentUserService _currentUserService)
        : SaveChangesInterceptor
    {
        private void UpdateAuditFields(DbContext? context)
        {
            if (context is null)
                return;

            var userId = _currentUserService.GetCurrentUserId();
            var now = DateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.ModifiedAt = now;

                    entry.Entity.CreatedBy = userId ?? Constants.DEFAULT_SYSTEM;
                    entry.Entity.ModifiedBy = userId ?? Constants.DEFAULT_SYSTEM;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(e => e.CreatedAt).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;

                    entry.Entity.ModifiedAt = now;
                    entry.Entity.ModifiedBy = userId ?? Constants.DEFAULT_SYSTEM;
                }
            }
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result
        )
        {
            UpdateAuditFields(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
        {
            UpdateAuditFields(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
