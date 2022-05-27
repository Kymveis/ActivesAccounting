using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using ActivesAccounting.Annotations;
using ActivesAccounting.Core.Utils;
using ActivesAccounting.Model.Contracts;

using Humanizer;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels;

internal abstract class AddItemViewModelBase<T> : INotifyPropertyChanged
{
    protected AddItemViewModelBase(
        Action aCloseAction,
        Action<T, int> aNewItemAction,
        bool aAllowDuplicates,
        string aItemName)
    {
        OkCommand = new RelayCommand(() =>
            processOkCommand(aCloseAction, aNewItemAction, aAllowDuplicates, aItemName));
        CancelCommand = new RelayCommand(aCloseAction);
    }

    protected abstract ITemplate<T> Template { get; }

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? aPropertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(aPropertyName));

    private void processOkCommand(
        Action aCloseAction,
        Action<T, int> aNewItemAction,
        bool aAllowDuplicates,
        string aItemName)
    {
        var errors = Template.CollectErrors();

        if (errors.Any())
        {
            DialogUtils.Error(
                "Validation error",
                errors.JoinStrings());
            return;
        }

        if (Template.IsDuplicate && breakDuplicateCreating())
        {
            return;
        }

        var (item, index) = Template.ToItem();
        aNewItemAction(item, index);
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
    }
}