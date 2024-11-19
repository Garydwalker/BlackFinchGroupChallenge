
using ApplicationDomain.Domain;
using ApplicationDomain.Events.LoanApplicationComplete;
using ApplicationDomain.Events.LoanApprovalRequest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationDomain.Stores;

public class LoanApplicationConfiguration : IEntityTypeConfiguration<LoanApplication>
{
    public void Configure(EntityTypeBuilder<LoanApplication> builder)
    {
        builder.ToContainer("LoanApplications");
        builder.HasKey(la => la.Id);
        builder.HasPartitionKey(x => x.Id);

        builder.HasDiscriminator<string>("Discriminator").HasValue<LoanApplication>("LoanApplication");
    }

}

public class BaseEventConfiguration : IEntityTypeConfiguration<BaseEvent>
{
    public void Configure(EntityTypeBuilder<BaseEvent> builder)
    {
        builder.ToContainer("LoanApplications");
        builder.HasKey(la => la.Id);
        builder.HasPartitionKey(x => x.Id);
        builder.HasDiscriminator<string>("Discriminator")
            .HasValue<BaseEvent>(nameof(BaseEvent))
            .HasValue<LoanApprovalRequestEvent>(nameof(LoanApprovalRequestEvent))
            .HasValue<LoanApplicationCompleteEvent>(nameof(LoanApplicationCompleteEvent))
            ;

    }
}
