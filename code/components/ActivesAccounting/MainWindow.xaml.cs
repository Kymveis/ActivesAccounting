using ActivesAccounting.Core;
using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.ViewModels.Implementation;

using Autofac;

namespace ActivesAccounting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        var container = createContainer();

        DataContext = new MainViewModel(new SessionViewModel(
            container.Resolve<ISessionFactory>(),
            container.Resolve<ISessionSerializer>(),
            container.Resolve<IBuilderFactory<IRecordBuilder>>(),
            container.Resolve<IBuilderFactory<IValueBuilder>>(),
            container.Resolve<IBuilderFactory<IPlatformBuilder>>(),
            container.Resolve<IBuilderFactory<ICurrencyBuilder>>()));

        IContainer createContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CoreAddIn>();

            return containerBuilder.Build();
        }
    }
}