using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using ActivesAccounting.Commands;
using ActivesAccounting.Core.Instantiating.Contracts;
using ActivesAccounting.Core.Instantiating.Contracts.Builders;
using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Serialization.Contracts;
using ActivesAccounting.Core.Utils;
using ActivesAccounting.Session.Implementation;
using ActivesAccounting.ViewModels.Contracts;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.ViewModels.Implementation;

internal sealed class SessionViewModel : ObservableObject, ISessionViewModel
{
    private readonly AppSession _session = new();

    private readonly ISessionFactory _sessionFactory;
    private readonly ISessionSerializer _sessionSerializer;

    public SessionViewModel(
        ISessionFactory aSessionFactory,
        ISessionSerializer aSessionSerializer,
        IBuilderFactory<IRecordBuilder> aRecordBuilderFactory,
        IBuilderFactory<IValueBuilder> aValueBuilderFactory,
        IBuilderFactory<IPlatformBuilder> aPlatformBuilderFactory,
        IBuilderFactory<ICurrencyBuilder> aCurrencyBuilderFactory)
    {
        _sessionFactory = aSessionFactory;
        _sessionSerializer = aSessionSerializer;

        var sessionCommandsContainer = new SessionCommandsContainer(_session);

        CreateSession = new RelayCommand(createNewSession);
        OpenSession = new AsyncRelayCommand(openSession);
        SaveSession = sessionCommandsContainer.CreateAsyncCommand(saveSession);

        AddRecord = sessionCommandsContainer.CreateCommand(addRecord);

        void createNewSession()
        {
            setSession(_sessionFactory.Create(), sessionCommandsContainer);
            _session.File = null;
        }

        async Task openSession()
        {
            if (!DialogUtils.TryOpenFile(out var file))
            {
                return;
            }

            _session.File = file;
            await using var stream = _session.File.OpenRead();

            setSession(
                await _sessionSerializer.DeserializeAsync(stream, CancellationToken.None),
                sessionCommandsContainer);
        }

        async Task saveSession()
        {
            if (_session.File is null)
            {
                if (!DialogUtils.TrySaveFile(out var file))
                {
                    return;
                }

                _session.File = file;
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
            var addRecordWindow = new AddRecordWindow(
                _session.ActualSession.Records,
                _session.ActualSession.Platforms,
                _session.ActualSession.Currencies,
                aRecordBuilderFactory,
                aValueBuilderFactory,
                aPlatformBuilderFactory,
                aCurrencyBuilderFactory);

            if (addRecordWindow.ShowWindow(out var record, out var index))
            {
                RecordViewModels.Insert(index!.Value, new RecordViewModel(record!));
            }
        }
    }

    public ICommand CreateSession { get; }
    public ICommand OpenSession { get; }
    public ICommand SaveSession { get; }

    public ICommand AddRecord { get; }

    public ObservableCollection<RecordViewModel> RecordViewModels { get; } = new();

    private void setSession(ISession aSession, SessionCommandsContainer aSessionCommandsContainer)
    {
        _session.ActualSession = aSession;
        aSessionCommandsContainer.NotifySessionChanging();

        RecordViewModels.Clear();
        aSession.Records.Select(aR => new RecordViewModel(aR)).ForEach(RecordViewModels.Add);
    }
}