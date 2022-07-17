using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Validation;

internal sealed class CurrencyValidator : NamedItemValidatorBase<ICurrency>
{
    public CurrencyValidator()
    {
        RuleFor(aCurrency => aCurrency.Type)
            .IsInEnum()
            .NotEqual(CurrencyType.Undefined);
    }
}