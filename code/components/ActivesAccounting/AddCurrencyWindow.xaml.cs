using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels.Implementation;

namespace ActivesAccounting;

public partial class AddCurrencyWindow : IAddItemWindow<ICurrency>
{
    private ICurrency? _currency;
    private int? _index;

    public AddCurrencyWindow(IContainer<ICurrency> aContainer, IBuilderFactory<ICurrencyBuilder> aBuilderFactory)
    {
        InitializeComponent();
        DataContext = new AddCurrencyViewModel(
            Close,
            (aC, aI) =>
            {
                _currency = aC;
                _index = aI;
            },
            aContainer,
            aBuilderFactory);
    }

    public bool ShowWindow(out ICurrency? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _currency;
        aIndex = _index;
        return _currency is not null;
    }
}