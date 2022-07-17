using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Validation;

internal sealed class RecordValidator : AbstractValidator<IRecord>
{
    public RecordValidator()
    {
        RuleFor(aRecord => aRecord.DateTime).NotNull();
        RuleFor(aRecord => aRecord.Source).NotNull();
        RuleFor(aRecord => aRecord.Target).NotNull();
        
        RuleFor(aRecord => aRecord.RecordType)
            .IsInEnum()
            .NotEqual(RecordType.Undefined);
    }
}