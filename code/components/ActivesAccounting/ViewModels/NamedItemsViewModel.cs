using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using ActivesAccounting.Annotations;
using ActivesAccounting.Core.Model.Contracts;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels;

internal sealed class NamedItemsViewModel<T> : INotifyPropertyChanged where T : INamedItem
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
    
    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? aPropertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));

    #endregion
}