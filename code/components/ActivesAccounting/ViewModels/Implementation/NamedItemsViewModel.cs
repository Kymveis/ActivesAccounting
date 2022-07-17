using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels.Contracts;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class NamedItemsViewModel<T> : ViewModelBase, INamedItemsViewModel<T> where T : INamedItem
{
    private T? _selectedItem;

    public NamedItemsViewModel(
        IEnumerable<T> aItems,
        Func<IAddItemWindow<T>> aAddWindowFactory,
        string aItemNames)
    {
        Items = new ObservableCollection<T>(aItems);
        CreateButtonText = $"Create {aItemNames}";

        CreateItem = new RelayCommand(() =>
        {
            var window = aAddWindowFactory();
            if (window.ShowWindow(out var newItem, out var index))
            {
                Items.Insert(index!.Value, newItem!);
                SelectedItem = newItem;
            }
        });
    }

    public ObservableCollection<T> Items { get; }
    public string CreateButtonText { get; }

    public T? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }

    public ICommand CreateItem { get; }
}