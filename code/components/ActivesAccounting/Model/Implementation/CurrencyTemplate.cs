using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Model.Contracts;

using FluentValidation;
using FluentValidation.Results;

namespace ActivesAccounting.Model.Implementation;

internal sealed class CurrencyTemplate : TemplateBase<ICurrency>, ICurrencyTemplate
{
    private sealed class CurrencyTemplateValidator : AbstractValidator<ICurrencyTemplate>
    {
        public CurrencyTemplateValidator()
        {
            RuleFor(aT => aT.Name).NotEmpty();
            RuleFor(aT => aT.Type)
                .NotEmpty()
                .NotEqual(CurrencyType.Undefined);
        }
    }

    private readonly CurrencyTemplateValidator _validator = new();
    private readonly ICurrenciesContainer _currenciesContainer;

    public CurrencyTemplate(ICurrenciesContainer aCurrenciesContainer)
    {
        _currenciesContainer = aCurrenciesContainer;
    }

    protected override bool IsDuplicateInternal =>
        _currenciesContainer.Currencies.Any(aC => aC.Name.Equals(Name));

    public string? Name { get; set; }
    public CurrencyType? Type { get; set; }

    protected override ValidationResult Validate() => _validator.Validate(this);
    protected override (ICurrency, int) ToItemInternal()
    {
        var currency = _currenciesContainer.CreateCurrency(Name!, Type!.Value);
        return (currency, IndexOf(currency, _currenciesContainer.Currencies));
    }
}