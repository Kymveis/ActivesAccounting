using System.Collections.ObjectModel;

using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.ViewModels.Contracts;

internal interface IAddCurrencyViewModel : IAddItemViewModel
{
    string Name { get; set; }
    CurrencyType SelectedType { get; set; }
    ObservableCollection<CurrencyType> CurrencyTypes { get; }
}