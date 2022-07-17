using System.Windows.Input;

namespace ActivesAccounting.ViewModels.Contracts;

internal interface IAddItemViewModel : IViewModel
{
    ICommand OkCommand { get; }
    ICommand CancelCommand { get; }
}