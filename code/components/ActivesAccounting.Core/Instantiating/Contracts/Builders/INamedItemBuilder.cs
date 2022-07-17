namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface INamedItemBuilder<out T> : IBuilder<T>
{
    void SetName(string aName);
}