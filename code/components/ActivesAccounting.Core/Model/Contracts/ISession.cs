using ActivesAccounting.Core.Instantiating.Contracts;

namespace ActivesAccounting.Core.Model.Contracts;

public interface ISession
{
    IContainer<IRecord> Records { get; }
    IContainer<ICurrency> Currencies { get; }
    IContainer<IPlatform> Platforms { get; }
}