using ValueType = ActivesAccounting.Core.Model.Enums.ValueType;

namespace ActivesAccounting.Core.Model.Contracts
{
    public interface ISimpleValue : IValue
    {
        ValueType IValue.ValueType => ValueType.Simple;
        ICurrency Currency { get; }
        decimal Count { get; }
    }
}