using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Utils;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class PricesContainer : ContainerBase<ICurrencyPrice>, IPricesContainer
{
    private record Price(ICurrency Exchanged, ICurrency Unit, decimal Count, PriceType Type, DateTime DateTime,
        Guid Guid) : ICurrencyPrice
    {
        public decimal Count { get; private set; } = Count;
        public PriceType Type { get; private set; } = Type;

        public void RecordExactCount(decimal aExactCount)
        {
            if (Type != PriceType.Calculated)
            {
                throw new InvalidOperationException("Exact count records possible only for modulated prices.");
            }

            Count = aExactCount.ValidateMoreThanZero(nameof(aExactCount));
            Type = PriceType.Recorded;
        }
    }

    public IEnumerable<ICurrencyPrice> Prices => Items;
    protected override string ItemName => "price";

    public bool TryGetPrice(
        ICurrency aExchanged,
        ICurrency aUnit,
        DateTime aDateTime,
        out ICurrencyPrice? aPrice)
    {
        var existingPrices = Items
            .Where(p => p.Exchanged.Equals(aExchanged) && p.Unit.Equals(aUnit) && p.DateTime.Equals(aDateTime))
            .ToArray();

        if (existingPrices.Length > 1)
        {
            throw new InvalidDataException($"{existingPrices.Length} prices matches the given arguments.");
        }

        aPrice = existingPrices.FirstOrDefault();
        return existingPrices.Length == 1;
    }

    public ICurrencyPrice CreatePrice(
        ICurrency aExchanged,
        ICurrency aUnit,
        decimal aCount,
        PriceType aType,
        DateTime aDateTime) =>
        createPrice(aExchanged, aUnit, aCount, aType, aDateTime, Guid.NewGuid());

    ICurrencyPrice IPricesContainer.CreatePrice(
        ICurrency aExchanged,
        ICurrency aUnit,
        decimal aCount,
        PriceType aType,
        DateTime aDateTime,
        Guid aGuid) =>
        createPrice(aExchanged, aUnit, aCount, aType, aDateTime, aGuid);

    ICurrencyPrice IPricesContainer.GetPrice(Guid aPriceGuid) => GetItemByGuid(aPriceGuid);

    private ICurrencyPrice createPrice(
        ICurrency aExchanged,
        ICurrency aUnit,
        decimal aCount,
        PriceType aType,
        DateTime aDateTime,
        Guid aGuid) =>
        AddItem(
            new Price(
                aExchanged,
                aUnit,
                aCount.ValidateMoreThanZero(),
                aType.ValidateEnum(PriceType.Undefined),
                aDateTime,
                aGuid), aGuid);
}