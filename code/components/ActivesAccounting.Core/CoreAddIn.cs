using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Implementation;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Serialization.Implementation;

using Autofac;

namespace ActivesAccounting.Core
{
    public sealed class CoreAddIn : Module
    {
        protected override void Load(ContainerBuilder aBuilder)
        {
            aBuilder.RegisterType<ValueFactory>().As<IValueFactory>().SingleInstance();
            aBuilder.RegisterType<CurrenciesContainer>().As<ICurrenciesContainer>().SingleInstance();
            aBuilder.RegisterType<PricesContainer>().As<IPricesContainer>().SingleInstance();
            aBuilder.RegisterType<RecordsContainer>().As<IRecordsContainer>().SingleInstance();
            aBuilder.RegisterType<SessionFactory>().As<ISessionFactory>().SingleInstance();
            aBuilder.RegisterType<SessionSerializer>().As<ISessionSerializer>().SingleInstance();
        }
    }
}