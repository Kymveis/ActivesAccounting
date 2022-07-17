using System.Linq;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Implementation;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal abstract class BuilderBase<TDeclared, TActual> : IBuilder<TDeclared> where TActual : TDeclared, new()
{
    private readonly IValidator<TDeclared> _validator;

    protected BuilderBase(IValidator<TDeclared> aValidator)
    {
        _validator = aValidator;
    }

    protected TActual Instance { get; } = new();

    public IResult<TDeclared> Build()
    {
        var validationResult = _validator.Validate(Instance);

        return validationResult.IsValid
            ? new Result<TDeclared>(Instance)
            : new Result<TDeclared>(validationResult.Errors.Select(aError => aError.ErrorMessage));
    }
}