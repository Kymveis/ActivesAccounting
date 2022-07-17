using ActivesAccounting.ViewModels.Contracts;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class MainViewModel : IMainViewModel
{
    public MainViewModel(ISessionViewModel aSessionViewModel)
    {
        SessionViewModel = aSessionViewModel;
    }

    public ISessionViewModel SessionViewModel { get; }
}