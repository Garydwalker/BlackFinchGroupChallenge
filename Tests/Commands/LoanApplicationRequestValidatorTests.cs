using FluentValidation.TestHelper;
using LoanApplicationApp.Commands;

namespace LoanApplicationApp.Tests.Commands;

[TestFixture]
public class LoanApplicationRequestValidatorTests
{
    private LoanApplicationRequestValidator _validator = null!;

    [SetUp]
    public void Setup()
    {
        _validator = new LoanApplicationRequestValidator();
    }

    [Test]
    public void Should_HaveError_When_LoanAmountIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest(null, "1000000", "750");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LoanAmount)
            .WithErrorMessage("Loan amount is required.");
    }

    [Test]
    public void Should_HaveError_When_AssetValueIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", null, "750");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AssetValue)
            .WithErrorMessage("Asset value is required.");
    }

    [Test]
    public void Should_HaveError_When_CreditScoreIsNull()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", null);

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreditScore)
            .WithErrorMessage("Credit score is required.");
    }

    [Test]
    public void Should_HaveError_When_CreditScoreIsOutOfRange()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "1000");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.CreditScore)
            .WithErrorMessage("Credit score must be between 1 and 999.");
    }

    [Test]
    public void Should_NotHaveError_When_RequestIsValid()
    {
        // Arrange
        var request = new LoanApplicationRequest("500000", "1000000", "750");

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.LoanAmount);
        result.ShouldNotHaveValidationErrorFor(x => x.AssetValue);
        result.ShouldNotHaveValidationErrorFor(x => x.CreditScore);
    }
}