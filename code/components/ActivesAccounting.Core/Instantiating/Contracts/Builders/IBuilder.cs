using ActivesAccounting.Core.Model.Contracts;

namespace ActivesAccounting.Core.Instantiating.Contracts.Builders;

public interface IBuilder<out T> : IBuilder
{
    IResult<T> Build();
}

public interface IBuilder
{
}