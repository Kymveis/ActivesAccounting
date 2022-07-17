using System;

using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;

using FluentValidation;

namespace ActivesAccounting.Core.Instantiating.Implementation.Builders;

internal abstract class UniqueItemBuilderBase<TDeclared, TActual> :
    BuilderBase<TDeclared, TActual>, IUniqueItemBuilder<TDeclared>
    where TDeclared : IUniqueItem
    where TActual : UniqueItemBuilderBase<TDeclared, TActual>.UniqueItemBase, TDeclared, new()
{
    internal abstract class UniqueItemBase : IUniqueItem
    {
        public Guid Guid { get; set; }
    }

    protected UniqueItemBuilderBase(IValidator<TDeclared> aValidator) : base(aValidator)
    {
        Instance.Guid = Guid.NewGuid();
    }

    void IUniqueItemBuilder<TDeclared>.SetGuid(Guid aGuid) => Instance.Guid = aGuid;
}