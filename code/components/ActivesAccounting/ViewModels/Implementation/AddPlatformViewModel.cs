using System;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.ViewModels.Contracts;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class AddPlatformViewModel : AddItemViewModelBase<IPlatform, IPlatformBuilder>, IAddPlatformViewModel
{
    private string _name = string.Empty;

    public AddPlatformViewModel(
        Action aCloseAction,
        Action<IPlatform, int> aNewItemAction,
        IContainer<IPlatform> aContainer,
        IBuilderFactory<IPlatformBuilder> aBuilderFactory)
        : base(aCloseAction, aNewItemAction, false, "Platform", aContainer, aBuilderFactory)
    {
    }

    public string Name
    {
        get => _name;
        set
        {
            SetProperty(ref _name, value);
            Builder.SetName(value);
        }
    }
}