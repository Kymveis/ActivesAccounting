using System;
using System.Globalization;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Model.Contracts;

using FluentValidation;
using FluentValidation.Results;

namespace ActivesAccounting.Model.Implementation;

internal sealed class RecordTemplate : TemplateBase<IRecord>, IRecordTemplate
{
    private class ValueTemplate
    {
        private readonly IValueFactory _valueFactory;

        public ValueTemplate(IValueFactory aValueFactory)
        {
            _valueFactory = aValueFactory;
        }

        public IPlatform? Platform { get; set; }
        public ICurrency? Currency { get; set; }
        public string? Value { get; set; }

        public IValue ToValue()
        {
            tryParseValue(Value, out var value);
            return _valueFactory.CreateValue(Platform!, Currency!, value);
        }

        public bool Equivalent(IValue aOther) =>
            aOther.Platform.Equals(Platform)
            && aOther.Currency.Equals(Currency)
            && tryParseValue(Value, out var value)
            && aOther.Count.Equals(value);
    }

    private sealed class RecordTemplateValidator : AbstractValidator<IRecordTemplate>
    {
        public RecordTemplateValidator()
        {
            RuleFor(aT => aT.DateTime).NotNull();
            RuleFor(aT => aT.Type)
                .NotNull()
                .NotEqual(RecordType.Undefined);
        }
    }

    private sealed class ValueTemplateValidator : AbstractValidator<ValueTemplate>
    {
        public ValueTemplateValidator()
        {
            RuleFor(aT => aT.Platform).NotNull();
            RuleFor(aT => aT.Currency).NotNull();
            RuleFor(aT => aT.Value)
                .Must(aT => tryParseValue(aT, out _));
        }
    }

    private readonly RecordTemplateValidator _recordValidator = new();
    private readonly ValueTemplateValidator _valueValidator = new();

    private readonly IRecordsContainer _recordsContainer;
    private readonly ValueTemplate _sourceValueTemplate;
    private readonly ValueTemplate _targetValueTemplate;

    public RecordTemplate(IRecordsContainer aRecordsContainer, IValueFactory aValueFactory)
    {
        _recordsContainer = aRecordsContainer;
        _sourceValueTemplate = new ValueTemplate(aValueFactory);
        _targetValueTemplate = new ValueTemplate(aValueFactory);
    }

    protected override bool IsDuplicateInternal =>
        _recordsContainer.Records.Any(aR =>
            aR.DateTime.Equals(DateTime)
            && aR.RecordType.Equals(Type)
            && _sourceValueTemplate.Equivalent(aR.Source)
            && _targetValueTemplate.Equivalent(aR.Target));

    public DateTime? DateTime { get; set; }
    public RecordType? Type { get; set; }

    public IPlatform? SourcePlatform
    {
        get => _sourceValueTemplate.Platform;
        set => _sourceValueTemplate.Platform = value;
    }

    public ICurrency? SourceCurrency
    {
        get => _sourceValueTemplate.Currency;
        set => _sourceValueTemplate.Currency = value;
    }

    public string? SourceValue
    {
        get => _sourceValueTemplate.Value;
        set => _sourceValueTemplate.Value = value;
    }

    public IPlatform? TargetPlatform
    {
        get => _targetValueTemplate.Platform;
        set => _targetValueTemplate.Platform = value;
    }

    public ICurrency? TargetCurrency
    {
        get => _targetValueTemplate.Currency;
        set => _targetValueTemplate.Currency = value;
    }

    public string? TargetValue
    {
        get => _targetValueTemplate.Value;
        set => _targetValueTemplate.Value = value;
    }

    protected override ValidationResult Validate()
    {
        var recordValidationResult = _recordValidator.Validate(this);
        var sourceValueValidationResult = _valueValidator.Validate(_sourceValueTemplate);
        var targetValueValidationResult = _valueValidator.Validate(_targetValueTemplate);

        return new ValidationResult(
            recordValidationResult.Errors
                .Concat(sourceValueValidationResult.Errors)
                .Concat(targetValueValidationResult.Errors));
    }

    protected override (IRecord, int) ToItemInternal()
    {
        var record = _recordsContainer.CreateRecord(
            DateTime!.Value,
            Type!.Value,
            _sourceValueTemplate.ToValue(),
            _targetValueTemplate.ToValue());
        
        return (record, IndexOf(record, _recordsContainer.Records));
    }

    private static bool tryParseValue(string? aValue, out decimal aDecimal)
    {
        return tryParseWithCulture(CultureInfo.InvariantCulture, out aDecimal)
            || tryParseWithCulture(CultureInfo.CurrentCulture, out aDecimal);

        bool tryParseWithCulture(IFormatProvider aCultureInfo, out decimal aCurrentCultureValue) =>
            decimal.TryParse(aValue, NumberStyles.Any, aCultureInfo, out aCurrentCultureValue);
    }
}