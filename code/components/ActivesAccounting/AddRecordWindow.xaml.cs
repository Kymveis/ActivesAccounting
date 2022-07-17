using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels.Implementation;

namespace ActivesAccounting;

/// <summary>
/// Interaction logic for AddRecordWindow.xaml
/// </summary>
public partial class AddRecordWindow : IAddItemWindow<IRecord>
{
    private IRecord? _record;
    private int? _index;

    public AddRecordWindow(
        IContainer<IRecord> aRecordsContainer,
        IContainer<IPlatform> aPlatformsContainer,
        IContainer<ICurrency> aCurrenciesContainer,
        IBuilderFactory<IRecordBuilder> aRecordBuilderFactory,
        IBuilderFactory<IValueBuilder> aValueBuilderFactory,
        IBuilderFactory<IPlatformBuilder> aPlatformBuilderFactory,
        IBuilderFactory<ICurrencyBuilder> aCurrencyBuilderFactory)
    {
        InitializeComponent();

        var sourcePlatformViewModel = createPlatformsViewModel(aPlatformsContainer, aPlatformBuilderFactory);
        var targetPlatformViewModel = createPlatformsViewModel(aPlatformsContainer, aPlatformBuilderFactory);
        var sourceCurrencyViewModel = createCurrenciesViewModel(aCurrenciesContainer, aCurrencyBuilderFactory);
        var targetCurrencyViewModel = createCurrenciesViewModel(aCurrenciesContainer, aCurrencyBuilderFactory);

        DataContext = new AddRecordViewModel(
            Close,
            (aR, aI) =>
            {
                _record = aR;
                _index = aI;
            },
            sourcePlatformViewModel,
            targetPlatformViewModel,
            sourceCurrencyViewModel,
            targetCurrencyViewModel,
            aRecordsContainer,
            aRecordBuilderFactory,
            aValueBuilderFactory);
    }

    public bool ShowWindow(out IRecord? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _record;
        aIndex = _index;
        return _record is not null;
    }

    private static NamedItemsViewModel<IPlatform> createPlatformsViewModel(
        IContainer<IPlatform> aPlatforms, IBuilderFactory<IPlatformBuilder> aBuilderFactory) => new(
        aPlatforms,
        () => new AddPlatformWindow(aPlatforms, aBuilderFactory),
        "Platform");

    private static NamedItemsViewModel<ICurrency> createCurrenciesViewModel(
        IContainer<ICurrency> aCurrencies, IBuilderFactory<ICurrencyBuilder> aBuilderFactory) => new(
        aCurrencies,
        () => new AddCurrencyWindow(aCurrencies, aBuilderFactory),
        "Currency");
}