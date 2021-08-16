using DotNetCoreCurrencyApi.Core.Models;
using FluentValidation;

namespace DotNetCoreCurrencyApi.Infrastructure.Validators
{
    public class ModelValidators
    {
        public class ExchangeTransactionModelValidator : AbstractValidator<ExchangeTransactionModel>
        {
            public ExchangeTransactionModelValidator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("User id must be provided");
                
                RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Transaction amount must be provided");

                RuleFor(x => x.CurrencyCode)
                    .NotEmpty().WithMessage("Currency code must be provided")
                    .Length(3).WithMessage("Currency code length is not valid");                    

            }
        }
    }
}
