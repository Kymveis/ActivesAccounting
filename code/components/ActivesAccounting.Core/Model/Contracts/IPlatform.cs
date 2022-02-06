namespace ActivesAccounting.Core.Model.Contracts
{
    public interface IPlatform : IUniqueItem
    {
        string Name { get; }
    }
}