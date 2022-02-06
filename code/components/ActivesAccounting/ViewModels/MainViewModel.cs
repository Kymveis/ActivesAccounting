using ActivesAccounting.Core;
using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Serialization.Contracts;

using Autofac;

using IContainer = Autofac.IContainer;

namespace ActivesAccounting.ViewModels
{
    internal sealed class MainViewModel
    {
        public MainViewModel()
        {
            var container = createContainer();

            SessionViewModel = new SessionViewModel(
                container.Resolve<IPlatformsContainer>(),
                container.Resolve<ICurrenciesContainer>(),
                container.Resolve<IPricesContainer>(),
                container.Resolve<IRecordsContainer>(),
                container.Resolve<ISessionFactory>(),
                container.Resolve<ISessionSerializer>(),
                container.Resolve<IValueFactory>());

            IContainer createContainer()
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterModule<CoreAddIn>();

                return containerBuilder.Build();
            }
        }

        public SessionViewModel SessionViewModel { get; }
    }
}