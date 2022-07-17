using System.Collections.ObjectModel;
using System.Windows.Input;

using ActivesAccounting.ViewModels.Implementation;

namespace ActivesAccounting.ViewModels.Contracts;

internal interface ISessionViewModel : IViewModel
{
    ICommand CreateSession { get; }
    ICommand OpenSession { get; }
    ICommand SaveSession { get; }
    ICommand AddRecord { get; }
    ObservableCollection<RecordViewModel> RecordViewModels { get; }
}