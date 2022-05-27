using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Model.Contracts;

namespace ActivesAccounting.Model.Implementation;

internal sealed class TemplateFactory : ICurrencyTemplateFactory, IPlatformTemplateFactory, IRecordTemplateFactory
{
    private readonly ICurrenciesContainer _currenciesContainer;
    private readonly IPlatformsContainer _platformsContainer;
    private readonly IRecordsContainer _recordsContainer;
    private readonly IValueFactory _valueFactory;

    public TemplateFactory(
        ICurrenciesContainer aCurrenciesContainer,
        IPlatformsContainer aPlatformsContainer,
        IRecordsContainer aRecordsContainer,
        IValueFactory aValueFactory)
    {
        _currenciesContainer = aCurrenciesContainer;
        _platformsContainer = aPlatformsContainer;
        _recordsContainer = aRecordsContainer;
        _valueFactory = aValueFactory;
    }

    ICurrencyTemplate ICurrencyTemplateFactory.Create() => new CurrencyTemplate(_currenciesContainer);

    IPlatformTemplate IPlatformTemplateFactory.Create() => new PlatformTemplate(_platformsContainer);

    IRecordTemplate IRecordTemplateFactory.Create() => new RecordTemplate(_recordsContainer, _valueFactory);
}