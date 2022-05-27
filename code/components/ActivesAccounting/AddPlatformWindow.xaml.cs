using System.Windows;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Model.Contracts;
using ActivesAccounting.ViewModels;

namespace ActivesAccounting;

public partial class AddPlatformWindow : Window, IAddItemWindow<IPlatform>
{
    private IPlatform? _platform;
    private int? _index;

    public AddPlatformWindow(IPlatformTemplateFactory aPlatformTemplateFactory)
    {
        InitializeComponent();
        DataContext = new AddPlatformViewModel(
            aPlatformTemplateFactory,
            Close,
            (aC, aI) =>
            {
                _platform = aC;
                _index = aI;
            });
    }

    public bool ShowWindow(out IPlatform? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _platform;
        aIndex = _index;
        return _platform is not null;
    }
}