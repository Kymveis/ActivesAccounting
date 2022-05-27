using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using ActivesAccounting.Core.Utils;
using ActivesAccounting.Model.Contracts;

using FluentValidation.Results;

namespace ActivesAccounting.Model.Implementation;

internal abstract class TemplateBase<TItem> : ITemplate<TItem>
{
    protected abstract bool IsDuplicateInternal { get; }

    protected abstract ValidationResult Validate();
    protected abstract (TItem Item, int Index) ToItemInternal();

    public bool IsDuplicate
    {
        get
        {
            assertValid();
            return IsDuplicateInternal;
        }
    }

    public IReadOnlyCollection<string> CollectErrors() =>
        Validate().Errors.Select(aE => aE.ErrorMessage).ToImmutableArray();

    public (TItem Item, int Index) ToItem()
    {
        assertValid();
        return ToItemInternal();
    }

    protected int IndexOf(TItem aItem, IEnumerable<TItem> aItems)
    {
        var index = 0;
        foreach (var item in aItems)
        {
            if (aItem!.Equals(item))
            {
                return index;
            }

            index++;
        }

        throw Exceptions.NotHasItem(typeof(TItem).Name, aItem);
    }

    private void assertValid()
    {
        if (!Validate().IsValid)
        {
            throw new InvalidOperationException($"{typeof(TItem).Name} template is invalid.");
        }
    }
}