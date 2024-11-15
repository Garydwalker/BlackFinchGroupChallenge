# Lending Platform
Build a simple Console application using the technology of your choice (preferably C#) that enables the writing and reporting of loans as per the requirements below. This should be approached as a way that can demonstrate your process to solving problems (any required infrastructure can simply be mocked), and does not need to be built to a production standard. Instead the exercise should be timeboxed to no longer than an hour. Notes can be taken of any assumptions made, and also any other considerations or improvements that you might make if this was a production application.

## Requirements
### User inputs that the application should require:
* Amount that the loan is for in GBP
* The value of the asset that the loan will be secured against
* The credit score of the applicant (between 1 and 999)

### Metrics that the application should output:
* Whether or not the applicant was successful
* The total number of applicants to date, broken down by their success status
* The total value of loans written to date
* The mean average Loan to Value of all applications received to date
    * Loan to Value (LTV) is the amount that the loan is for, expressed as a percentage of the value of the asset that the loan will be secured against.

### Business rules used to derive whether or not the applicant was successful:
* If the value of the loan is more than £1.5 million or less than £100,000 then the application must be declined
* If the value of the loan is £1 million or more then the LTV must be 60% or less and the credit score of the applicant must be 950 or more
* If the value of the loan is less than £1 million then the following rules apply:
    * If the LTV is less than 60%, the credit score of the applicant must be 750 or more
    * If the LTV is less than 80%, the credit score of the applicant must be 800 or more
    * If the LTV is less than 90%, the credit score of the applicant must be 900 or more
    * If the LTV is 90% or more, the application must be declined

## Assumptions
1. Rounding for LTV should be to 2 decimal places.

## Considerations
* I felt a real evaluation of the Applcation could take time and likely comumicate with 3rd party services. so i didnt want the evaluation to be part of the creation of the application. 
* I felt the "stats" requirement had nothing to do with the loan application nor the evaluation of the application. so the code should be seperated.

## Improvements
* Create a persistance DataStore - something like Azure Comsos would all the messages to become asynchronous and the outbox pattern could be used to ensure the an atomic action of creating the Application and raising the events. or maybe use the transactional outbox pattern with Dapr.
* It feels that i have created several services within this project that would be better suited to seperate projects. these could be seperated out to their own domain.  Reporting and Application.  the evaluation process could feasibly be seperated as well.


