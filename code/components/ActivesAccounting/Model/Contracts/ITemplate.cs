using System.Collections.Generic;

namespace ActivesAccounting.Model.Contracts;

public interface ITemplate<TItem>
{
    bool IsDuplicate { get; }
    IReadOnlyCollection<string> CollectErrors();
    (TItem Item, int Index) ToItem();
}