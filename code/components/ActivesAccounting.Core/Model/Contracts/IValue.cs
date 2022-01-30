using ActivesAccounting.Core.Model.Enums;

namespace ActivesAccounting.Core.Model.Contracts
{
    public interface IValue
    {
        ValueType ValueType { get; }
    }
}