namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface IBuilderFactory<out T> where T : IBuilder
{
    T Create();
}