using ApplicationDomain.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApplicationDomain.Stores;

public class LoanApplicationStore(ApplicationDbContext dbContext)
{
    public virtual async Task<LoanApplication?> Get(Guid applicationId)
    {
        await EnsureCreated();
        return await dbContext.LoanApplications.FirstOrDefaultAsync(application => application.Id == applicationId);
    }

    private async Task<bool> EnsureCreated()
    {
        
        return await dbContext.Database.EnsureCreatedAsync();
    }

    public virtual async Task Create(LoanApplication application)
    {
        await EnsureCreated();
        await  dbContext.AddAsync(application);
    }
}