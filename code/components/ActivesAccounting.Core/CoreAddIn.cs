using System.Text.Json.Serialization;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Implementation;
using ActivesAccounting.Core.Instantiating.Implementation.Builders;
using ActivesAccounting.Core.Instantiating.Validation;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Serialization.Converters;
using ActivesAccounting.Core.Serialization.Implementation;

using Autofac;

using FluentValidation;

namespace ActivesAccounting.Core;

public sealed class CoreAddIn : Module
{
    protected override void Load(ContainerBuilder aBuilder)
    {
        aBuilder.RegisterType<BuilderFactory>().AsImplementedInterfaces().SingleInstance();
        aBuilder.RegisterType<SessionFactory>().As<ISessionFactory>().SingleInstance();
        aBuilder.RegisterType<SessionSerializer>().As<ISessionSerializer>().SingleInstance();

        registerValidators(aBuilder);
        registerJsonConverters(aBuilder);
    }

    private static void registerValidators(ContainerBuilder aBuilder)
    {
        aBuilder.RegisterType<CurrencyValidator>().As<IValidator<ICurrency>>();
        aBuilder.RegisterType<PlatformValidator>().As<IValidator<IPlatform>>();
        aBuilder.RegisterType<RecordValidator>().As<IValidator<IRecord>>();
        aBuilder.RegisterType<ValueValidator>().As<IValidator<IValue>>();
    }

    private static void registerJsonConverters(ContainerBuilder aBuilder)
    {
        aBuilder.RegisterType<SessionConverter>().As<JsonConverter>();
        aBuilder.RegisterType<CurrencyConverter>().As<JsonConverter>();
        aBuilder.RegisterType<RecordConverter>().As<JsonConverter>();
        aBuilder.RegisterType<PlatformConverter>().As<JsonConverter>();
        aBuilder.RegisterType<ValueConverter>().As<JsonConverter>();
    }
}