// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
//
// using ActivesAccounting.Core.Instantiating.Implementation;
//
// namespace ActivesAccounting.BusinessLogic;

//public sealed class CurrencyPrices
//{
//    private record TimePrices
//    {
//        public IPrice LatestBefore { get; set; }
//        public LinkedList<IPrice> Exact { get; } = new();
//        public IPrice EarliestAfter { get; set; }
//    }

//    private readonly PricesContainer _pricesContainer;

//    public CurrencyPrices(PricesContainer aPrices) =>
//        _pricesContainer = aPrices ?? throw new ArgumentNullException(nameof(aPrices));

//    public Task<bool> CanGetPriceAsync(ICurrency aUnit, ICurrency aValue, CancellationToken aCancellationToken)
//    {
//        if (aUnit is null)
//        {
//            throw new ArgumentNullException(nameof(aUnit));
//        }

//        if (aValue is null)
//        {
//            throw new ArgumentNullException(nameof(aValue));
//        }

//        return Task.Run(getSuitablePrices(aUnit, aValue).Any, aCancellationToken);
//    }

//    public async Task<IPrice> GetPriceAsync(ICurrency aUnit, ICurrency aValue, DateTime aDateTime,
//        CancellationToken aCancellationToken)
//    {
//        var prices = await Task.Run(() => getSuitablePrices(aUnit, aValue)
//            .Aggregate(new TimePrices(), (timePrices, price) =>
//            {
//                var priceDateTime = price.DateTime;
//                switch (priceDateTime.CompareTo(aDateTime))
//                {
//                    case -1 when timePrices.LatestBefore is null
//                        || timePrices.LatestBefore.DateTime < priceDateTime:
//                        timePrices.LatestBefore = price;
//                        break;
//                    case 0:
//                        timePrices.Exact.AddLast(price);
//                        break;
//                    case 1 when timePrices.EarliestAfter is null
//                        || timePrices.EarliestAfter.DateTime > priceDateTime:
//                        timePrices.EarliestAfter = price;
//                        break;
//                }

//                return timePrices;
//            }), aCancellationToken);

//        var exactTimePrices = prices.Exact;
//        return exactTimePrices.Count switch
//        {
//            0 => getNonExactPrice(aUnit, aValue, aDateTime, prices.LatestBefore, prices.EarliestAfter),
//            1 => exactTimePrices.First(),
//            _ => exactTimePrices.FirstOrDefault(p => p.Type == PriceType.Recorded) ?? exactTimePrices.First()
//        };
//    }

//    private IEnumerable<IPrice> getSuitablePrices(ICurrency aUnit, ICurrency aValue)
//    {
//        return _pricesContainer
//            .Items
//            .AsParallel()
//            .Select(getExactPriceOrDefault)
//            .Where(p => p is not null);

//        IPrice getExactPriceOrDefault(IPrice aPrice)
//        {
//            if (matches(aPrice, aUnit, aValue))
//            {
//                return aPrice;
//            }

//            if (matches(aPrice, aValue, aUnit))
//            {
//                return _pricesContainer.CreatePrice(aUnit, aValue, 1 / aPrice.Count, PriceType.Recorded,
//                    aPrice.DateTime);
//            }

//            return default;
//        }

//        static bool matches(IPrice aPrice, ICurrency aMatchUnit, ICurrency aMatchValue) =>
//            aPrice.Exchanged.Equals(aMatchUnit) && aPrice.Unit.Equals(aMatchValue);
//    }

//    private IPrice getNonExactPrice(
//        ICurrency aUnit,
//        ICurrency aValue,
//        DateTime aDateTime,
//        IPrice aPriceBefore,
//        IPrice aPriceAfter)
//    {
//        var hasPriceAfter = aPriceAfter is not null;
//        return (aPriceBefore is not null) switch
//        {
//            true when hasPriceAfter => createModulatedPrice(),
//            true when !hasPriceAfter => aPriceBefore,
//            false when hasPriceAfter => aPriceAfter,
//            _ => throw new InvalidOperationException("Cannot get a price.")
//        };

//        IPrice createModulatedPrice() => _pricesContainer.CreatePrice(
//            aUnit,
//            aValue,
//            modulatePriceCount(
//                aPriceAfter.DateTime.Ticks,
//                aPriceBefore.DateTime.Ticks,
//                aPriceAfter.Count,
//                aPriceBefore.Count),
//            PriceType.Modulated, aDateTime);

//        double modulatePriceCount(
//            double aTimeAfter,
//            double aTimeBefore,
//            double aCountAfter,
//            double aCountBefore)
//        {
//            var timeDifference = aTimeAfter - aTimeBefore;

//            return calculateCoefficient() * aDateTime.Ticks + calculateShift();

//            double calculateCoefficient() => (aCountAfter - aCountBefore) / timeDifference;

//            double calculateShift() =>
//                aTimeAfter * aCountBefore / timeDifference
//                - aTimeBefore * aCountAfter / timeDifference;
//        }
//    }
//}