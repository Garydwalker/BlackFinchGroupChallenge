using BlazorApp1.Components.Pages;
using Bunit;
using FluentAssertions;
using TestContext = Bunit.TestContext;

namespace BlazorAppTests;

public class CreateApplicationTests : TestContext
{
    [Test]
    public void CreateApplicationRendersCorrectly()
    {
        // Arrange
        var component = RenderComponent<CreateApplication>();

        // Act
        var loanAmountInput = component.Find("#amount");
        var assetValueInput = component.Find("#asset-value");
        var creditScoreInput = component.Find("#credit-score");
        var submitButton = component.Find("button[type=submit]");

        // Assert
        loanAmountInput.Should().NotBeNull();
        assetValueInput.Should().NotBeNull();
        creditScoreInput.Should().NotBeNull();
        submitButton.Should().NotBeNull();
    }

    [Test]
    public void FormSubmissionDisplaysRequiredValidationMessages()
    {
        // Arrange
        var component = RenderComponent<CreateApplication>();

        // Act
        var loanAmountInput = component.Find("#amount");
            loanAmountInput.Change(string.Empty);
        var assetValueInput = component.Find("#asset-value");
        assetValueInput.Change(string.Empty);
        var creditScoreInput = component.Find("#credit-score");
        creditScoreInput.Change(string.Empty);
        component.Find("button[type=submit]").Click();

        // Assert
        component.Markup.Should().Contain("The LoanAmount field must be a number");
        component.Markup.Should().Contain("The AssetValue field must be a number");
        component.Markup.Should().Contain("The CreditScore field must be a number");
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void FormSubmissionDisplaysNonValidNumberValidationMessages(decimal value)
    {
        // Arrange
        var component = RenderComponent<CreateApplication>();

        // Act
        var loanAmountInput = component.Find("#amount");
        loanAmountInput.Change(value);
        var assetValueInput = component.Find("#asset-value");
        assetValueInput.Change(value);
        component.Find("button[type=submit]").Click();

        // Assert
        component.Markup.Should().Contain("Loan Amount must be greater than 0.");
        component.Markup.Should().Contain("Asset Value must be greater than 0.");
    }

    [TestCase(0)]
    [TestCase(1000)]
    public void FormSubmissionDisplaysInvalidRangeForCreditScoreValidationMessages(decimal value)
    {
        // Arrange
        var component = RenderComponent<CreateApplication>();

        // Act
        var creditScoreInput = component.Find("#credit-score");
        creditScoreInput.Change(value);
        component.Find("button[type=submit]").Click();

        // Assert
        component.Markup.Should().Contain("Credit Score must be between 1 and 999.");
        component.Markup.Should().Contain("Asset Value must be greater than 0.");
    }
    [Test]
    public void ValidFormSubmission()
    {
        // Arrange
        var component = RenderComponent<CreateApplication>();

        // Act
        var loanAmountInput = component.Find("#amount");
        loanAmountInput.Change(1000);
        var assetValueInput = component.Find("#asset-value");
        assetValueInput.Change(2000);
        var creditScoreInput = component.Find("#credit-score");
        creditScoreInput.Change(750);
        component.Find("button[type=submit]").Click();

        // Assert
        component.Markup.Should().NotContain("Loan Amount is required.");
        component.Markup.Should().NotContain("Asset Value is required.");
        component.Markup.Should().NotContain("Credit Score is required.");
        component.Markup.Should().NotContain("Loan Amount must be greater than 0.");
        component.Markup.Should().NotContain("Asset Value must be greater than 0.");
        component.Markup.Should().NotContain("Credit Score must be between 1 and 999.");

        var model = component.Instance.Application;
        model.LoanAmount.Should().Be(1000);
        model.AssetValue.Should().Be(2000);
        model.CreditScore.Should().Be(750);
    }
}