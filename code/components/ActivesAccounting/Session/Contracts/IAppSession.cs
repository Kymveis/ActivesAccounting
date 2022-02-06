using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Session.Contracts
{
    internal interface IAppSession
    {
        bool IsSessionOpen { get; }
        ISession ActualSession { get; }
    }
}