using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Model.Contracts;

namespace ActivesAccounting.ViewModels;

internal sealed class AddRecordViewModel : AddItemViewModelBase<IRecord>
{
    private sealed record RecordTemplate( IRecordsContainer RecordsContainer,
        DateTime Date, RecordType Type, IValue SourceValue, IValue TargetValue) : ITemplate<IRecord>
    {
        public bool HasDuplicate => RecordsContainer
            .Records
            .Any(aR =>
                aR.DateTime.Equals(Date)
                && aR.RecordType.Equals(Type)
                && aR.Source.Equals(SourceValue)
                && aR.Target.Equals(TargetValue));

        public IRecord ToItem() => RecordsContainer.CreateRecord(Date, Type, SourceValue, SourceValue);
    }

    private readonly IRecordsContainer _recordsContainer;
    private readonly IValueFactory _valueFactory;
    private RecordType _selectedType;
    private IPlatform? _sourcePlatform;
    private IPlatform? _targetPlatform;
    private ICurrency? _sourceCurrency;
    private ICurrency? _targetCurrency;
    private string? _sourceValue;
    private string? _targetValue;

    public AddRecordViewModel(
        IPlatformsContainer aPlatformsContainer,
        ICurrenciesContainer aCurrenciesContainer,
        IRecordsContainer aRecordsContainer,
        IValueFactory aValueFactory,
        Action aCloseAction,
        Action<IRecord> aSetNewRecordAction)
        : base(aCloseAction, aSetNewRecordAction, true, "Record")
    {
        _recordsContainer = aRecordsContainer;
        _valueFactory = aValueFactory;
        Types = new ObservableCollection<RecordType>(new[]
        {
            RecordType.Deposit,
            RecordType.Transfer,
            RecordType.Withdrawal
        });

        SelectedType = Types[0];

        Platforms = new ObservableCollection<IPlatform>(aPlatformsContainer.Platforms);
        Currencies = new ObservableCollection<ICurrency>(aCurrenciesContainer.Currencies);
        SourceCurrency = new CurrenciesViewModel(aCurrenciesContainer);
    }

    public DateTime Date { get; set; } = DateTime.Today;

    #region Types

    public ObservableCollection<RecordType> Types { get; }

    public RecordType SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Platforms

    public ObservableCollection<IPlatform> Platforms { get; }

    public IPlatform? SourcePlatform
    {
        get => _sourcePlatform;
        set
        {
            _sourcePlatform = value;
            OnPropertyChanged();
        }
    }

    public IPlatform? TargetPlatform
    {
        get => _targetPlatform;
        set
        {
            _targetPlatform = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Currencies

    public CurrenciesViewModel SourceCurrency { get; }

    public ObservableCollection<ICurrency> Currencies { get; }

    public ICurrency? TargetCurrency
    {
        get => _targetCurrency;
        set
        {
            _targetCurrency = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Values

    public string? SourceValue
    {
        get => _sourceValue;
        set
        {
            _sourceValue = value;
            OnPropertyChanged();
        }
    }

    public string? TargetValue
    {
        get => _targetValue;
        set
        {
            _targetValue = value;
            OnPropertyChanged();
        }
    }

    #endregion

    protected override ITemplate<IRecord> ValidateFields(ICollection<string> aErrors)
    {
        validateValues(
            SourcePlatform,
            default,
            SourceValue,
            "Source",
            out var sourceCount);

        validateValues(
            TargetPlatform,
            TargetCurrency,
            TargetValue,
            "Target",
            out var targetCount);

        return aErrors.Any()
            ? CreateInvalidTemplate()
            : new RecordTemplate(_recordsContainer, Date, SelectedType,
                _valueFactory.CreateValue(SourcePlatform!, default!, sourceCount),
                _valueFactory.CreateValue(TargetPlatform!, TargetCurrency!, targetCount));

        void validateValues(
            IPlatform? aPlatform,
            ICurrency? aCurrency,
            string? aValue,
            string aDescription,
            out decimal aParsedValue)
        {
            if (aPlatform is null)
            {
                aErrors.Add($"{aDescription} platform is not selected.");
            }

            if (aCurrency is null)
            {
                aErrors.Add($"{aDescription} currency is not selected.");
            }

            var parsedValue = default(decimal);
            if (aValue is null)
            {
                aErrors.Add($"{aDescription} value is not entered.");
            }
            else if (!tryParseValue(CultureInfo.InvariantCulture, out parsedValue)
                     && !tryParseValue(CultureInfo.CurrentCulture, out parsedValue))
            {
                aErrors.Add($"{aDescription} value is not a number.");
            }

            aParsedValue = parsedValue;

            bool tryParseValue(IFormatProvider aCultureInfo, out decimal aCurrentCultureValue) =>
                decimal.TryParse(aValue, NumberStyles.Any, aCultureInfo, out aCurrentCultureValue);
        }
    }
}