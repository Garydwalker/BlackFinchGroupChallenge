﻿
using LoanDecisionApi.LoanApprovalEngine.Rules;

namespace LoanDecisionApi.LoanApprovalEngine;

public class LoanApplicationApprovalEngine(IEnumerable<ILoanAcceptanceRule> rules)
{
    public virtual Task<bool> Evaluate(LoanApplication application)
    {
        return Task.FromResult(rules.All(loanAcceptanceRule => loanAcceptanceRule.Evaluate(application)));
    }
}