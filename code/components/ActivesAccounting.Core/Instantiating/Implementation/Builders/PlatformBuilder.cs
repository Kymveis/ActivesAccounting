using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal sealed class PlatformBuilder : UniqueItemBuilderBase<IPlatform, PlatformBuilder.Platform>, IPlatformBuilder
{
    internal sealed class Platform : UniqueItemBase, IPlatform
    {
        public string Name { get; set; } = string.Empty;
    }

    public PlatformBuilder(IValidator<IPlatform> aValidator) : base(aValidator)
    {
    }

    public void SetName(string aName) => Instance.Name = aName;
}