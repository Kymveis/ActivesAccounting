using System.Windows;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Model.Contracts;
using ActivesAccounting.ViewModels;

namespace ActivesAccounting;

public partial class AddCurrencyWindow : Window, IAddItemWindow<ICurrency>
{
    private ICurrency? _currency;
    private int? _index;

    public AddCurrencyWindow(ICurrencyTemplateFactory aCurrencyTemplateFactory)
    {
        InitializeComponent();
        DataContext = new AddCurrencyViewModel(
            aCurrencyTemplateFactory,
            Close,
            (aC, aI) =>
            {
                _currency = aC;
                _index = aI;
            });
    }

    public bool ShowWindow(out ICurrency? aItem, out int? aIndex)
    {
        ShowDialog();
        aItem = _currency;
        aIndex = _index;
        return _currency is not null;
    }
}