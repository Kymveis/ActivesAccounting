using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Model.Contracts;

namespace ActivesAccounting.ViewModels;

internal sealed class AddRecordViewModel : AddItemViewModelBase<IRecord>
{
    private readonly IRecordTemplate _template;

    public AddRecordViewModel(
        IRecordTemplateFactory aRecordTemplateFactory,
        NamedItemsViewModel<IPlatform> aSourcePlatformViewModel,
        NamedItemsViewModel<IPlatform> aTargetPlatformViewModel,
        NamedItemsViewModel<ICurrency> aSourceCurrencyViewModel,
        NamedItemsViewModel<ICurrency> aTargetCurrencyViewModel,
        Action aCloseAction,
        Action<IRecord, int> aSetNewRecordAction)
        : base(aCloseAction, aSetNewRecordAction, true, "Record")
    {
        _template = aRecordTemplateFactory.Create();
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

        SourcePlatformViewModel.PropertyChanged += (_, _) =>
            _template.SourcePlatform = SourcePlatformViewModel.SelectedItem;

        SourceCurrencyViewModel.PropertyChanged += (_, _) =>
            _template.SourceCurrency = SourceCurrencyViewModel.SelectedItem;

        TargetPlatformViewModel.PropertyChanged += (_, _) =>
            _template.TargetPlatform = TargetPlatformViewModel.SelectedItem;

        TargetCurrencyViewModel.PropertyChanged += (_, _) =>
            _template.TargetCurrency = TargetCurrencyViewModel.SelectedItem;

        pairItemCollectionViews(SourcePlatformViewModel, TargetPlatformViewModel);
        pairItemCollectionViews(SourceCurrencyViewModel, TargetCurrencyViewModel);

        void pairItemCollectionViews<T>(NamedItemsViewModel<T> aSource, NamedItemsViewModel<T> aTarget)
            where T : INamedItem
        {
            pairItemCollections(aSource, aTarget);
            pairItemCollections(aTarget, aSource);

            void pairItemCollections(NamedItemsViewModel<T> aLeft, NamedItemsViewModel<T> aRight) =>
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

    protected override ITemplate<IRecord> Template => _template;

    public NamedItemsViewModel<IPlatform> SourcePlatformViewModel { get; }

    public NamedItemsViewModel<IPlatform> TargetPlatformViewModel { get; }

    public NamedItemsViewModel<ICurrency> SourceCurrencyViewModel { get; }

    public NamedItemsViewModel<ICurrency> TargetCurrencyViewModel { get; }

    public ObservableCollection<RecordType> Types { get; }

    public DateTime? Date
    {
        get => _template.DateTime;
        set => _template.DateTime = value;
    }

    public RecordType? SelectedType
    {
        get => _template.Type;
        set => _template.Type = value;
    }

    public string? SourceValue
    {
        get => _template.SourceValue;
        set => _template.SourceValue = value;
    }

    public string? TargetValue
    {
        get => _template.TargetValue;
        set => _template.TargetValue = value;
    }
}