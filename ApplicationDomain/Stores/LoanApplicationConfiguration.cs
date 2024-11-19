
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

        builder.HasDiscriminator<string>("$type").HasValue<LoanApplication>("LoanApplication");
    }

}

public class BaseEventConfiguration : IEntityTypeConfiguration<BaseEvent>
{
    public void Configure(EntityTypeBuilder<BaseEvent> builder)
    {
        builder.ToContainer("LoanApplications");
        builder.HasKey(la => la.Id);
        builder.HasPartitionKey(x => x.Id);
        builder.HasDiscriminator<string>("$type")
            .HasValue<BaseEvent>(nameof(BaseEvent))
            .HasValue<LoanApprovalRequestEvent>(nameof(LoanApprovalRequestEvent))
            .HasValue<LoanApplicationCompleteEvent>(nameof(LoanApplicationCompleteEvent))
            ;

    }
}
//
// public class LoanApprovalRequestEventConfiguration : IEntityTypeConfiguration<LoanApprovalRequestEvent>
// {
//     public void Configure(EntityTypeBuilder<LoanApprovalRequestEvent> builder)
//     {
//         builder.HasPartitionKey(x=>x.ApplicationId);
//     }
// }
//
// public class LoanApplicationCompleteEventConfiguration : IEntityTypeConfiguration<LoanApplicationCompleteEvent>
// {
//     public void Configure(EntityTypeBuilder<LoanApplicationCompleteEvent> builder)
//     {
//         builder.HasPartitionKey(x => x.ApplicationId);
//         
//     }
// }