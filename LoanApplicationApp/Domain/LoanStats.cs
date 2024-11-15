namespace LoanApplicationApp.Domain;

public class LoanStats
{
    public int TotalApplications => SuccessfulApplications + UnsuccessfulApplications;
    public int SuccessfulApplications { get; private set; }
    public int UnsuccessfulApplications { get; private set; }
    public decimal TotalValueOfLoans { get; private set; }
    public decimal AverageLtv { get; private set; }

    public void UpdateStats(LoanApplication application)
    {
        AverageLtv = Math.Round((AverageLtv * TotalApplications + application.LoanToValuePercentage) / (TotalApplications + 1), 2);

        if (application.ApprovalStatus == true)
        {
            SuccessfulApplications++;
            TotalValueOfLoans += application.Amount;
        }
        else
        {
            UnsuccessfulApplications++;
        }
    }
}