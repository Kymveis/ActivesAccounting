using System;
using System.Collections.ObjectModel;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.ViewModels.Contracts;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class AddCurrencyViewModel : AddItemViewModelBase<ICurrency, ICurrencyBuilder>, IAddCurrencyViewModel
{
    private string _name = string.Empty;
    private CurrencyType _selectedType;

    public AddCurrencyViewModel(
        Action aCloseAction,
        Action<ICurrency, int> aNewItemAction,
        IContainer<ICurrency> aContainer,
        IBuilderFactory<ICurrencyBuilder> aBuilderFactory)
        : base(aCloseAction, aNewItemAction, false, "Currency", aContainer, aBuilderFactory)
    {
        CurrencyTypes = new ObservableCollection<CurrencyType>(
            new[] {CurrencyType.Fiat, CurrencyType.Stock, CurrencyType.Crypto});
        SelectedType = CurrencyTypes[0];
    }

    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            Builder.SetName(value);
        }
    }

    public CurrencyType SelectedType
    {
        get => _selectedType;
        set
        {
            SetProperty(ref _selectedType, value);
            Builder.SetType(value);
        }
    }

    public ObservableCollection<CurrencyType> CurrencyTypes { get; }
}