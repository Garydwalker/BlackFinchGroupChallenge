using ApplicationDomain.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Stores;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply the LoanApplication configuration
        modelBuilder.ApplyConfiguration(new LoanApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new BaseEventConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if ((entry.State != EntityState.Added && entry.State != EntityState.Modified)) continue;
            if (entry.Entity is not DomainEntity domainEntity) continue;

            var events = domainEntity.Events.ToList();
            domainEntity.ClearEvents();
            await AddRangeAsync(events, cancellationToken);
            
        }

        foreach (var entry in ChangeTracker.Entries())
        {
            var test = entry.Entity;
        }

        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}