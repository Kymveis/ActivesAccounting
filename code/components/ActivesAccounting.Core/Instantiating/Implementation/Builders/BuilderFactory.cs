using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal sealed class BuilderFactory : IBuilderFactory<ICurrencyBuilder>, IBuilderFactory<IPlatformBuilder>,
    IBuilderFactory<IRecordBuilder>, IBuilderFactory<IValueBuilder>
{
    private readonly IValidator<ICurrency> _currencyValidator;
    private readonly IValidator<IPlatform> _platformValidator;
    private readonly IValidator<IRecord> _recordValidator;
    private readonly IValidator<IValue> _valueValidator;

    public BuilderFactory(
        IValidator<ICurrency> aCurrencyValidator, 
        IValidator<IPlatform> aPlatformValidator, 
        IValidator<IRecord> aRecordValidator, 
        IValidator<IValue> aValueValidator)
    {
        _currencyValidator = aCurrencyValidator;
        _platformValidator = aPlatformValidator;
        _recordValidator = aRecordValidator;
        _valueValidator = aValueValidator;
    }

    ICurrencyBuilder IBuilderFactory<ICurrencyBuilder>.Create() => new CurrencyBuilder(_currencyValidator);

    IPlatformBuilder IBuilderFactory<IPlatformBuilder>.Create() => new PlatformBuilder(_platformValidator);

    IRecordBuilder IBuilderFactory<IRecordBuilder>.Create() => new RecordBuilder(_recordValidator);

    IValueBuilder IBuilderFactory<IValueBuilder>.Create() => new ValueBuilder(_valueValidator);
}