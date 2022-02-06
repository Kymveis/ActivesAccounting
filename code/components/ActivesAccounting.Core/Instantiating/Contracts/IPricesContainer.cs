using System;
using System.Collections.Generic;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Instantiating.Contracts
{
    public interface IPricesContainer : IContainer
    {
        IEnumerable<ICurrencyPrice> Prices { get; }

        bool TryGetPrice(
            ICurrency aExchanged,
            ICurrency aUnit,
            DateTime aDateTime,
            out ICurrencyPrice? aPrice);

        ICurrencyPrice CreatePrice(
            ICurrency aExchanged,
            ICurrency aUnit,
            decimal aCount,
            PriceType aType,
            DateTime aDateTime);

        internal ICurrencyPrice CreatePrice(
            ICurrency aExchanged,
            ICurrency aUnit,
            decimal aCount,
            PriceType aType,
            DateTime aDateTime,
            Guid aGuid);

        internal ICurrencyPrice GetPrice(Guid aPriceGuid);
    }
}