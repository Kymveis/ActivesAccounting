using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Model.Implementation;
using ActivesAccounting.ViewModels.Contracts;

using Humanizer;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class AddRecordViewModel : AddItemViewModelBase<IRecord, IRecordBuilder>, IAddRecordViewModel
{
    private readonly IValueBuilder _sourceValueBuilder;
    private readonly IValueBuilder _targetValueBuilder;

    private DateTime? _date;
    private RecordType? _selectedType;
    private string? _sourceValue;
    private string? _targetValue;

    public AddRecordViewModel(
        Action aCloseAction,
        Action<IRecord, int> aSetNewRecordAction,
        INamedItemsViewModel<IPlatform> aSourcePlatformViewModel,
        INamedItemsViewModel<IPlatform> aTargetPlatformViewModel,
        INamedItemsViewModel<ICurrency> aSourceCurrencyViewModel,
        INamedItemsViewModel<ICurrency> aTargetCurrencyViewModel,
        IContainer<IRecord> aContainer,
        IBuilderFactory<IRecordBuilder> aRecordBuilderFactory,
        IBuilderFactory<IValueBuilder> aValueBuilderFactory)
        : base(aCloseAction, aSetNewRecordAction, true, "Record", aContainer, aRecordBuilderFactory)
    {
        SourcePlatformViewModel = aSourcePlatformViewModel;
        TargetPlatformViewModel = aTargetPlatformViewModel;
        SourceCurrencyViewModel = aSourceCurrencyViewModel;
        TargetCurrencyViewModel = aTargetCurrencyViewModel;

        Types = new ObservableCollection<RecordType>(new[]
        {
            RecordType.Deposit,
            RecordType.Transfer,
            RecordType.Withdrawal
        });

        Date = DateTime.Today;
        SelectedType = Types[0];

        _sourceValueBuilder = aValueBuilderFactory.Create();
        _targetValueBuilder = aValueBuilderFactory.Create();

        SourcePlatformViewModel.PropertyChanged += (_, _) =>
            setIfNotNull(SourcePlatformViewModel.SelectedItem, _sourceValueBuilder.SetPlatform);

        SourceCurrencyViewModel.PropertyChanged += (_, _) =>
            setIfNotNull(SourceCurrencyViewModel.SelectedItem, _sourceValueBuilder.SetCurrency);

        TargetPlatformViewModel.PropertyChanged += (_, _) =>
            setIfNotNull(TargetPlatformViewModel.SelectedItem, _targetValueBuilder.SetPlatform);

        TargetCurrencyViewModel.PropertyChanged += (_, _) =>
            setIfNotNull(TargetCurrencyViewModel.SelectedItem, _targetValueBuilder.SetCurrency);

        pairItemCollectionViews(SourcePlatformViewModel, TargetPlatformViewModel);
        pairItemCollectionViews(SourceCurrencyViewModel, TargetCurrencyViewModel);

        void pairItemCollectionViews<T>(INamedItemsViewModel<T> aSource, INamedItemsViewModel<T> aTarget)
            where T : INamedItem
        {
            pairItemCollections(aSource, aTarget);
            pairItemCollections(aTarget, aSource);

            void pairItemCollections(INamedItemsViewModel<T> aLeft, INamedItemsViewModel<T> aRight) =>
                aLeft.Items.CollectionChanged += (_, aArgs) =>
                {
                    if (aArgs.Action is not NotifyCollectionChangedAction.Add)
                    {
                        throw new ArgumentOutOfRangeException(nameof(aArgs), aArgs, default);
                    }

                    var newItem = aArgs.NewItems!.Cast<T>().Single();
                    var targetIndex = aArgs.NewStartingIndex;
                    var collectionToUpdate = aRight.Items;

                    if (collectionToUpdate.Count <= targetIndex || !collectionToUpdate[targetIndex].Equals(newItem))
                    {
                        collectionToUpdate.Add(newItem);
                    }
                };
        }
    }

    public INamedItemsViewModel<IPlatform> SourcePlatformViewModel { get; }

    public INamedItemsViewModel<IPlatform> TargetPlatformViewModel { get; }

    public INamedItemsViewModel<ICurrency> SourceCurrencyViewModel { get; }

    public INamedItemsViewModel<ICurrency> TargetCurrencyViewModel { get; }

    public ObservableCollection<RecordType> Types { get; }

    public DateTime? Date
    {
        get => _date;
        set
        {
            SetProperty(ref _date, value);
            setIfNotNull(value, aDateTime => Builder.SetDateTime(aDateTime!.Value));
        }
    }

    public RecordType? SelectedType
    {
        get => _selectedType;
        set
        {
            SetProperty(ref _selectedType, value);
            setIfNotNull(value, aRecordType => Builder.SetRecordType(aRecordType!.Value));
        }
    }

    public string? SourceValue
    {
        get => _sourceValue;
        set => SetProperty(ref _sourceValue, value);
    }

    public string? TargetValue
    {
        get => _targetValue;
        set => SetProperty(ref _targetValue, value);
    }

    protected override IResult<IRecord> BuildItem()
    {
        var errors = new List<string>();

        buildValue(SourceValue, nameof(SourceValue), _sourceValueBuilder, Builder.SetSourceValue);
        buildValue(TargetValue, nameof(TargetValue), _targetValueBuilder, Builder.SetTargetValue);

        return errors.Any()
            ? new Result<IRecord>(errors)
            : Builder.Build();

        void buildValue(
            string? aPropertyValue,
            string aPropertyName,
            IValueBuilder aBuilder,
            Action<IValue> aSetValueAction)
        {
            if (!decimal.TryParse(aPropertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var count))
            {
                errors.Add($"{aPropertyName.Humanize(LetterCasing.Title)} is not a number.");
                return;
            }

            aBuilder.SetCount(count);

            var valueResult = aBuilder.Build();
            if (valueResult.Valid)
            {
                aSetValueAction(valueResult.Value);
            }
            else
            {
                errors.AddRange(valueResult.Errors.Select(aError =>
                    $"{aPropertyName.Humanize(LetterCasing.Title)} validation error: {aError}"));
            }
        }
    }

    private static void setIfNotNull<T>(T? aValue, Action<T> aSetAction)
    {
        if (aValue is not null)
        {
            aSetAction(aValue);
        }
    }
}