using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels.Implementation;

namespace ActivesAccounting;

public partial class AddPlatformWindow : IAddItemWindow<IPlatform>
{
    private IPlatform? _platform;
    private int? _index;

    public AddPlatformWindow(IContainer<IPlatform> aContainer, IBuilderFactory<IPlatformBuilder> aBuilderFactory)
    {
        InitializeComponent();
        DataContext = new AddPlatformViewModel(
            Close,
            (aC, aI) =>
            {
                _platform = aC;
                _index = aI;
            },
            aContainer,
            aBuilderFactory);
    }

    public bool ShowWindow(out IPlatform? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _platform;
        aIndex = _index;
        return _platform is not null;
    }
}