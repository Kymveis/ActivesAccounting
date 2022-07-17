using System.Collections.ObjectModel;
using System.Windows.Input;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.ViewModels.Contracts;

internal interface INamedItemsViewModel<T> : IViewModel where T : INamedItem
{
    ObservableCollection<T> Items { get; }
    string CreateButtonText { get; }
    T? SelectedItem { get; set; }
    ICommand CreateItem { get; }
}