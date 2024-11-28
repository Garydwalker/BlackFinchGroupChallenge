using LoanApplicationApp.Commands;
using LoanApplicationApp.Stores;
using MediatR;

namespace LoanApplicationApp;

public class LoanRequestService(IMediator mediator, LoanStatsStore loanStats)
{
    public async Task StartAsync()
    {
        while (true)
        {
            var selectedOptionCharValue = DisplayMenu();
            Console.WriteLine(Environment.NewLine);
            switch (selectedOptionCharValue)
            {
                case "1":
                {
                    await StartLoanApplicationAsync();
                    break;
                }
                case "2":
                {
                    ViewLoanStats();
                    break;
                }
                case "3":
                {
                    return;
                }
                default:
                    Console.WriteLine("Please select a valid option");
                    break;
            }
            Console.WriteLine(Environment.NewLine);
        }
    }

    private static string? DisplayMenu()
    {
        Console.WriteLine("System Options:");
        Console.WriteLine("1: Create New Loan Application");
        Console.WriteLine("2: View All Loan Statistics");
        Console.WriteLine("3: Exit");
        Console.WriteLine("Please Select an option");
        return Console.ReadLine();
    }

    private async Task StartLoanApplicationAsync()
    {
        Console.WriteLine("Please Enter a Loan Amount (in GBP)");
        var requestedLoanAmount = Console.ReadLine();
        Console.WriteLine("Please Enter the value of the asset that the loan will be secured against (in GBP)");
        var assetValue = Console.ReadLine();
        Console.WriteLine("Please Enter the credit score of the applicant (between 1 and 999)");
        var creditScore = Console.ReadLine();

        Console.WriteLine("Please wait whilst we review your application");
        await mediator.Send(new LoanApplicationRequest(requestedLoanAmount, assetValue, creditScore));
    }

    private void ViewLoanStats()
    {
        var stats = loanStats.GetStats();
        Console.WriteLine($"Total Applications To Date           : {stats.TotalApplications}");
        Console.WriteLine($"Total Successful                     : {stats.SuccessfulApplications}");
        Console.WriteLine($"Total Unsuccessful                   : {stats.UnsuccessfulApplications}");
        Console.WriteLine($"Total Value Of Loans Written         : {stats.TotalValueOfLoans}");
        Console.WriteLine($"Mean Average LTV Of All Applications : {stats.AverageLtv}");
        
    }
}