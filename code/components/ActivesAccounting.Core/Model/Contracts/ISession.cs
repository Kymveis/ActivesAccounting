using System.Collections.Generic;

namespace ActivesAccounting.Core.Model.Contracts;

public interface ISession
{
    IEnumerable<IRecord> Records { get; }
    IEnumerable<ICurrencyPrice> Prices { get; }
    IEnumerable<ICurrency> Currencies { get; }
    IEnumerable<IPlatform> Platforms { get; }
}