using System;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Model.Contracts;

namespace ActivesAccounting.ViewModels;

internal sealed class AddPlatformViewModel : AddItemViewModelBase<IPlatform>
{
    private readonly IPlatformTemplate _template;

    public AddPlatformViewModel(
        IPlatformTemplateFactory aPlatformTemplateFactory,
        Action aCloseAction,
        Action<IPlatform, int> aNewItemAction)
        : base(aCloseAction, aNewItemAction, false, "Platform")
    {
        _template = aPlatformTemplateFactory.Create();
    }

    protected override ITemplate<IPlatform> Template => _template;

    public string? Name
    {
        get => _template.Name;
        set => _template.Name = value;
    }
}