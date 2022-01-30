namespace ActivesAccounting.Core.Model.Contracts
{
    public interface ICurrency : IUniqueItem
    {
        string Name { get; }
    }
}