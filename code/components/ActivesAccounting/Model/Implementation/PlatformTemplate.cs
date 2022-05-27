using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Model.Contracts;

using FluentValidation;
using FluentValidation.Results;

namespace ActivesAccounting.Model.Implementation;

internal sealed class PlatformTemplate : TemplateBase<IPlatform>, IPlatformTemplate
{
    private sealed class PlatformTemplateValidator : AbstractValidator<IPlatformTemplate>
    {
        public PlatformTemplateValidator()
        {
            RuleFor(aT => aT.Name).NotEmpty();
        }
    }

    private readonly PlatformTemplateValidator _validator = new();
    private readonly IPlatformsContainer _platformsContainer;

    public PlatformTemplate(IPlatformsContainer aPlatformsContainer)
    {
        _platformsContainer = aPlatformsContainer;
    }

    protected override bool IsDuplicateInternal =>
        _platformsContainer.Platforms.Any(aP => aP.Name.Equals(Name));

    public string? Name { get; set; }
    
    protected override ValidationResult Validate() => _validator.Validate(this);
    protected override (IPlatform, int) ToItemInternal()
    {
        var platform = _platformsContainer.CreatePlatform(Name!);
        return (platform, IndexOf(platform, _platformsContainer.Platforms));
    }
}