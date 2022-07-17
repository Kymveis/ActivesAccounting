using System;
using System.Collections.Generic;
using System.Windows.Input;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;
using ActivesAccounting.ViewModels.Contracts;

using Humanizer;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels.Implementation;

internal abstract class AddItemViewModelBase<TItem, TBuilder> : ViewModelBase, IAddItemViewModel
    where TItem : IUniqueItem where TBuilder : IBuilder<TItem>
{
    protected AddItemViewModelBase(
        Action aCloseAction,
        Action<TItem, int> aNewItemAction,
        bool aAllowDuplicates,
        string aItemName,
        IContainer<TItem> aContainer,
        IBuilderFactory<TBuilder> aBuilderFactory)
    {
        Builder = aBuilderFactory.Create();
        OkCommand = new RelayCommand(() =>
            processOkCommand(aCloseAction, aNewItemAction, aAllowDuplicates, aItemName, aContainer));
        CancelCommand = new RelayCommand(aCloseAction);
    }

    protected TBuilder Builder { get; }

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    protected virtual IResult<TItem> BuildItem() => Builder.Build();

    private void processOkCommand(
        Action aCloseAction,
        Action<TItem, int> aNewItemAction,
        bool aAllowDuplicates,
        string aItemName,
        IContainer<TItem> aContainer)
    {
        var result = BuildItem();

        if (!result.Valid)
        {
            DialogUtils.Error("Validation error", result.Errors.JoinStrings());
            return;
        }

        if (aContainer.HasDuplicate(result.Value) && breakDuplicateCreating())
        {
            return;
        }

        aContainer.Add(result.Value);
        aNewItemAction(result.Value, indexOf(result.Value, aContainer));
        aCloseAction();

        bool breakDuplicateCreating()
        {
            if (aAllowDuplicates)
            {
                return !DialogUtils.Ask(
                    $"The {aItemName} already exists",
                    $"The {aItemName.Pluralize()} list already contains the same {aItemName}, do you want do create another one?");
            }

            DialogUtils.Error("Duplicate creating", $"The same {aItemName} already exists.");
            return true;
        }

        static int indexOf(TItem aItem, IEnumerable<TItem> aItems)
        {
            var index = 0;
            foreach (var item in aItems)
            {
                if (aItem.Guid.Equals(item.Guid))
                {
                    return index;
                }

                index++;
            }

            throw Exceptions.NotHasItem(typeof(TItem).Name, aItem);
        }
    }
}