using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Validation;

internal abstract class NamedItemValidatorBase<T> : AbstractValidator<T> where T : INamedItem
{
    protected NamedItemValidatorBase()
    {
        RuleFor(aNamedItem => aNamedItem.Name).NotEmpty();
    }
}