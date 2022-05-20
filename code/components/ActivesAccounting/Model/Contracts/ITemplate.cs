namespace ActivesAccounting.Model.Contracts;

internal interface ITemplate<out TItem>
{
    bool HasDuplicate { get; }
    TItem ToItem();
}