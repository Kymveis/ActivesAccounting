using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Model.Contracts;

public interface IPlatformTemplate : ITemplate<IPlatform>
{
    string? Name { get; set; }
}