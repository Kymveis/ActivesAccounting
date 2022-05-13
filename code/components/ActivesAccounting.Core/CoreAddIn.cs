using System.Text.Json.Serialization;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Implementation;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Serialization.Converters;
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
            aBuilder.RegisterType<PlatformsContainer>().As<IPlatformsContainer>().SingleInstance();
            aBuilder.RegisterType<PricesContainer>().As<IPricesContainer>().SingleInstance();
            aBuilder.RegisterType<RecordsContainer>().As<IRecordsContainer>().SingleInstance();
            aBuilder.RegisterType<SessionFactory>().As<ISessionFactory>().SingleInstance();
            aBuilder.RegisterType<SessionSerializer>().As<ISessionSerializer>().SingleInstance();

            registerJsonConverters(aBuilder);
        }

        private static void registerJsonConverters(ContainerBuilder aBuilder)
        {
            aBuilder.RegisterType<SessionConverter>().As<JsonConverter>();
            aBuilder.RegisterType<CurrencyConverter>().As<JsonConverter>();
            aBuilder.RegisterType<CurrencyPriceConverter>().As<JsonConverter>();
            aBuilder.RegisterType<RecordConverter>().As<JsonConverter>();
            aBuilder.RegisterType<PlatformConverter>().As<JsonConverter>();
            aBuilder.RegisterType<ValueConverter>().As<JsonConverter>();
        }
    }
}