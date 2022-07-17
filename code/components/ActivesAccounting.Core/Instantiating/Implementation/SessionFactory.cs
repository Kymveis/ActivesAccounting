using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Implementation;

internal sealed class SessionFactory : ISessionFactory
{
    private sealed class Session : ISession
    {
        public IContainer<IRecord> Records { get; } = new RecordsContainer();
        public IContainer<ICurrency> Currencies { get; } = new CurrenciesContainer();
        public IContainer<IPlatform> Platforms { get; } = new PlatformsContainer();
    }

    public ISession Create() => new Session();
}