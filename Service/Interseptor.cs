using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TaskMate.Entities.Common;

namespace TaskMate.Service
{
    public class Interseptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
              DbContextEventData eventData,
              InterceptionResult<int> result,
              CancellationToken cancellationToken = default)
        {   
            UpdateDateProperties(eventData.Context.ChangeTracker.Entries());
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private static void UpdateDateProperties(IEnumerable<EntityEntry> entries)
        {
            var now = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added && entry.Entity is BaseEntity)
                {
                    var entity = (BaseEntity)entry.Entity;
                    entity.CreatedDate = now;
                    entity.ModiffiedDate = now;
                }
                else if (entry.State == EntityState.Modified && entry.Entity is BaseEntity)
                {
                    var entity = (BaseEntity)entry.Entity;
                    entity.ModiffiedDate = now;
                }
            }
        }
    }
}