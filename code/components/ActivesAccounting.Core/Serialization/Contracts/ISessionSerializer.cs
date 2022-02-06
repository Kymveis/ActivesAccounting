using System.IO;
using System.Threading;
using System.Threading.Tasks;

using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Serialization.Contracts
{
    public interface ISessionSerializer
    {
        Task SerializeAsync(Stream aStream, ISession aSession, CancellationToken aCancellationToken);
        Task<ISession> DeserializeAsync(Stream aStream, CancellationToken aCancellationToken);
    }
}