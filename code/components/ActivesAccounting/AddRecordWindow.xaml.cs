using System.Windows;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Model.Contracts;
using ActivesAccounting.ViewModels;

namespace ActivesAccounting;

/// <summary>
/// Interaction logic for AddRecordWindow.xaml
/// </summary>
public partial class AddRecordWindow : Window, IAddItemWindow<IRecord>
{
    private IRecord? _record;
    private int? _index;
    
    public AddRecordWindow(
        IPlatformsContainer aPlatformsContainer,
        ICurrenciesContainer aCurrenciesContainer,
        IPlatformTemplateFactory aPlatformTemplateFactory,
        ICurrencyTemplateFactory aCurrencyTemplateFactory,
        IRecordTemplateFactory aRecordTemplateFactory)
    {
        InitializeComponent();

        var sourcePlatformViewModel = createPlatformsViewModel(aPlatformsContainer, aPlatformTemplateFactory);
        var targetPlatformViewModel = createPlatformsViewModel(aPlatformsContainer, aPlatformTemplateFactory);
        var sourceCurrencyViewModel = createCurrenciesViewModel(aCurrenciesContainer, aCurrencyTemplateFactory);
        var targetCurrencyViewModel = createCurrenciesViewModel(aCurrenciesContainer, aCurrencyTemplateFactory);
        
        DataContext = new AddRecordViewModel(
            aRecordTemplateFactory,
            sourcePlatformViewModel,
            targetPlatformViewModel,
            sourceCurrencyViewModel,
            targetCurrencyViewModel,
            Close,
            (aR, aI) =>
            {
                _record = aR;
                _index = aI;
            });
    }

    public bool ShowWindow(out IRecord? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _record;
        aIndex = _index;
        return _record is not null;
    }

    private static NamedItemsViewModel<IPlatform> createPlatformsViewModel(
        IPlatformsContainer aPlatformsContainer,
        IPlatformTemplateFactory aPlatformTemplateFactory) => new(
        aPlatformsContainer.Platforms,
        () => new AddPlatformWindow(aPlatformTemplateFactory),
        "Platform");

    private static NamedItemsViewModel<ICurrency> createCurrenciesViewModel(
        ICurrenciesContainer aCurrenciesContainer,
        ICurrencyTemplateFactory aCurrencyTemplateFactory) => new(
        aCurrenciesContainer.Currencies,
        () => new AddCurrencyWindow(aCurrencyTemplateFactory),
        "Currency");
}