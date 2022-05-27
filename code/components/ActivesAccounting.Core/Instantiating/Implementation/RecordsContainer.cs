using System;
using System.Collections.Generic;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class RecordsContainer : ContainerBase<IRecord>, IRecordsContainer
{
    private sealed record Record(DateTime DateTime, RecordType RecordType,
        IValue Source, IValue Target, IValue? Commission, Guid Guid) : IRecord;

    private readonly IPricesContainer _pricesContainer;

    public RecordsContainer(IPricesContainer aPricesContainer) =>
        _pricesContainer = aPricesContainer ?? throw new ArgumentNullException(nameof(aPricesContainer));

    protected override string ItemName => "record";
    public IReadOnlySet<IRecord> Records => Items;

    protected override IEnumerable<IComparable> GetComparableProperties(IRecord aItem)
    {
        yield return aItem.DateTime;
        yield return aItem.RecordType;
        yield return aItem.Source.Platform.Name;
        yield return aItem.Source.Currency.Name;
        yield return aItem.Target.Platform.Name;
        yield return aItem.Target.Currency.Name;
    }

    public IRecord CreateRecord(
        DateTime aDateTime,
        RecordType aRecordType,
        IValue aSource,
        IValue aTarget,
        IValue? aCommission = null) =>
        createRecord(aDateTime, aRecordType, aSource, aTarget, aCommission, Guid.NewGuid());

    IRecord IRecordsContainer.CreateRecord(
        DateTime aDateTime,
        RecordType aRecordType,
        IValue aSource,
        IValue aTarget,
        IValue? aCommission,
        Guid aGuid) =>
        createRecord(aDateTime, aRecordType, aSource, aTarget, aCommission, aGuid);

    private IRecord createRecord(
        DateTime aDateTime,
        RecordType aRecordType,
        IValue aSource,
        IValue aTarget,
        IValue? aCommission,
        Guid aGuid)
    {
        var record = Add(
            new Record(
                aDateTime,
                aRecordType.ValidateEnum(RecordType.Undefined),
                aSource,
                aTarget,
                aCommission,
                aGuid));

        var exchangedCurrency = aTarget.Currency;
        var unitCurrency = aSource.Currency;
        if (exchangedCurrency != unitCurrency)
        {
            _pricesContainer.CreatePrice(
                exchangedCurrency,
                unitCurrency,
                calculateCount(),
                PriceType.Recorded,
                aDateTime);
        }

        return record;

        decimal calculateCount()
        {
            var exchangedCount = aTarget.Count;
            var unitCount = aSource.Count;
            if (aCommission is null)
            {
                return unitCount / exchangedCount;
            }

            var commissionCurrency = aCommission.Currency;
            if (commissionCurrency != unitCurrency)
            {
                throw new NotImplementedException(
                    $"{commissionCurrency.Name} to {unitCurrency.Name} cross-currency commissions calculating is not implemented.");
            }

            var commissionCount = aCommission.Count;
            return (unitCount - commissionCount) / exchangedCount;
        }
    }
}