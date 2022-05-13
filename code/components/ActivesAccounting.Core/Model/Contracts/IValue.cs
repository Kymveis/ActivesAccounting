namespace ActivesAccounting.Core.Model.Contracts
{
    public interface IValue
    {
        ICurrency Currency { get; }
        decimal Count { get; }
        IPlatform Platform { get; }
    }
}