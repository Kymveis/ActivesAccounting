using System;
using System.Collections.ObjectModel;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.ViewModels.Contracts;

internal interface IAddRecordViewModel : IAddItemViewModel
{
    INamedItemsViewModel<IPlatform> SourcePlatformViewModel { get; }
    INamedItemsViewModel<IPlatform> TargetPlatformViewModel { get; }
    INamedItemsViewModel<ICurrency> SourceCurrencyViewModel { get; }
    INamedItemsViewModel<ICurrency> TargetCurrencyViewModel { get; }
    ObservableCollection<RecordType> Types { get; }
    DateTime? Date { get; set; }
    RecordType? SelectedType { get; set; }
    string? SourceValue { get; set; }
    string? TargetValue { get; set; }
}