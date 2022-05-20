using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using ActivesAccounting.Annotations;
using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels;

internal sealed class CurrenciesViewModel : INotifyPropertyChanged
{
    private ICurrency? _selectedCurrency;

    public CurrenciesViewModel(ICurrenciesContainer aCurrenciesContainer)
    {
        Currencies = new ObservableCollection<ICurrency>(aCurrenciesContainer.Currencies);
        
        CreateCurrency = new RelayCommand(() =>
        {
            var window = new AddCurrencyWindow(aCurrenciesContainer);
            window.ShowDialog();
            if (window.NewCurrency is not null)
            {
                Currencies.Add(window.NewCurrency);
                SelectedCurrency = window.NewCurrency;
            }
        });
    }

    public ObservableCollection<ICurrency> Currencies { get; }

    public ICurrency? SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _selectedCurrency = value;
            OnPropertyChanged();
        }
    }

    public ICommand CreateCurrency { get; }

    #region INotifyPropertyChanged implementation

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    private void OnPropertyChanged([CallerMemberName] string? aPropertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));

    #endregion
}