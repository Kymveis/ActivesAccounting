using System;
using System.Collections.ObjectModel;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Model.Contracts;

namespace ActivesAccounting.ViewModels;

internal sealed class AddCurrencyViewModel : AddItemViewModelBase<ICurrency>
{
    private readonly ICurrencyTemplate _template;

    public AddCurrencyViewModel(
        ICurrencyTemplateFactory aCurrencyTemplateFactory,
        Action aCloseAction,
        Action<ICurrency, int> aNewItemAction)
        : base(aCloseAction, aNewItemAction, false, "Currency")
    {
        _template = aCurrencyTemplateFactory.Create();

        CurrencyTypes = new ObservableCollection<CurrencyType>(
            new[] {CurrencyType.Fiat, CurrencyType.Stock, CurrencyType.Crypto});
        SelectedType = CurrencyTypes[0];
    }

    protected override ITemplate<ICurrency> Template => _template;

    public string? Name
    {
        get => _template.Name;
        set => _template.Name = value;
    }

    public CurrencyType? SelectedType
    {
        get => _template.Type;
        set => _template.Type = value;
    }

    public ObservableCollection<CurrencyType> CurrencyTypes { get; }
}