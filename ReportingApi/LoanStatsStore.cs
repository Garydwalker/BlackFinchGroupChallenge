namespace ReportingApi;

public class LoanStatsStore
{
    private readonly LoanStats _stats = new();

    public void Update(LoanApplication loanApplication)
    {
        _stats.UpdateStats(loanApplication);
    }
    public LoanStats GetStats()
    {
        return _stats;
    }
}