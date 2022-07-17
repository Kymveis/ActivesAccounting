using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Validation;

internal sealed class ValueValidator : AbstractValidator<IValue>
{
    public ValueValidator()
    {
        RuleFor(aValue => aValue.Platform).NotNull();
        RuleFor(aValue => aValue.Currency).NotNull();
        RuleFor(aValue => aValue.Count).GreaterThan(0);
    }
}