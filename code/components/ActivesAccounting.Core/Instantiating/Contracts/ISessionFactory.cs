using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts;

public interface ISessionFactory
{
    ISession Create();
}