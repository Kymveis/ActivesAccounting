using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels;

namespace ActivesAccounting;

/// <summary>
/// Interaction logic for AddRecordWindow.xaml
/// </summary>
public partial class AddRecordWindow : Window
{
    public AddRecordWindow(
        IPlatformsContainer aPlatformsContainer,
        ICurrenciesContainer aCurrenciesContainer,
        IRecordsContainer aRecordsContainer,
        IValueFactory aValueFactory)
    {
        InitializeComponent();
        DataContext = new AddRecordViewModel(
            aPlatformsContainer,
            aCurrenciesContainer,
            aRecordsContainer,
            aValueFactory,
            Close,
            aR => CreatedRecord = aR);
    }
    
    public IRecord? CreatedRecord { get; private set; }
}