using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ActivesAccounting.Commands;
using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Model.Enums;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Utils;
using ActivesAccounting.Session.Implementation;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels;

internal sealed class SessionViewModel : ObservableObject
{
    private readonly AppSession _session = new();

    private readonly IPlatformsContainer _platformsContainer;
    private readonly ICurrenciesContainer _currenciesContainer;
    private readonly IPricesContainer _pricesContainer;
    private readonly IRecordsContainer _recordsContainer;
    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionSerializer _sessionSerializer;
    private readonly IValueFactory _valueFactory;

    public SessionViewModel(
        IPlatformsContainer aPlatformsContainer,
        ICurrenciesContainer aCurrenciesContainer,
        IPricesContainer aPricesContainer,
        IRecordsContainer aRecordsContainer,
        ISessionFactory aSessionFactory,
        ISessionSerializer aSessionSerializer,
        IValueFactory aValueFactory)
    {
        _platformsContainer = aPlatformsContainer;
        _currenciesContainer = aCurrenciesContainer;
        _pricesContainer = aPricesContainer;
        _recordsContainer = aRecordsContainer;
        _sessionFactory = aSessionFactory;
        _sessionSerializer = aSessionSerializer;
        _valueFactory = aValueFactory;

        var sessionCommandsContainer = new SessionCommandsContainer(_session);

        CreateSession = new RelayCommand(createNewSession);
        OpenSession = new AsyncRelayCommand(openSession);
        SaveSession = sessionCommandsContainer.CreateAsyncCommand(saveSession);

        AddRecord = sessionCommandsContainer.CreateCommand(addRecord);

        void createNewSession()
        {
            clearContainers();
            setSession(_sessionFactory.CreateSession(
                    _recordsContainer,
                    _pricesContainer,
                    _currenciesContainer,
                    _platformsContainer),
                sessionCommandsContainer);
            _session.File = null;
        }

        async Task openSession()
        {
            if (DialogUtils.TryOpenFile(out var file))
            {
                _session.File = file!;
                await using var stream = _session.File.OpenRead();
                clearContainers();
                setSession(
                    await _sessionSerializer.DeserializeAsync(stream, CancellationToken.None),
                    sessionCommandsContainer);
            }
        }

        async Task saveSession()
        {
            if (_session.File is null)
            {
                if (!DialogUtils.TrySaveFile(out var file))
                {
                    return;
                }

                _session.File = file!;
            }

            _session.File.Delete();

            await using var fileStream = _session.File.Create();
            await _sessionSerializer.SerializeAsync(
                fileStream,
                _session.ActualSession,
                CancellationToken.None);
        }

        void addRecord()
        {
            var record = createFakeRecord();
            RecordViewModels.Add(new RecordViewModel(record));
        }
    }

    public ICommand CreateSession { get; }
    public ICommand OpenSession { get; }
    public ICommand SaveSession { get; }

    public ICommand AddRecord { get; }

    public ObservableCollection<RecordViewModel> RecordViewModels { get; } = new();

    private IRecord createFakeRecord() =>
        _recordsContainer.CreateRecord(
            DateTime.Today,
            RecordType.Transfer,
            _valueFactory.CreateValue(
                _session.ActualSession.Platforms.FirstOrDefault()
                ?? _platformsContainer.CreatePlatform("HODL"),
                _session.ActualSession.Currencies.FirstOrDefault()
                ?? _currenciesContainer.CreateCurrency("BITOK", CurrencyType.Crypto),
                1),
            _valueFactory.CreateValue(_session.ActualSession.Platforms.First(),
                _session.ActualSession.Currencies.First(), 1));

    private void setSession(ISession aSession, SessionCommandsContainer aSessionCommandsContainer)
    {
        _session.ActualSession = aSession;
        aSessionCommandsContainer.NotifySessionChanging();

        RecordViewModels.Clear();
        aSession.Records.Select(aR => new RecordViewModel(aR)).ForEach(RecordViewModels.Add);
    }

    private void clearContainers()
    {
        _platformsContainer.Clear();
        _currenciesContainer.Clear();
        _pricesContainer.Clear();
        _recordsContainer.Clear();
    }
}