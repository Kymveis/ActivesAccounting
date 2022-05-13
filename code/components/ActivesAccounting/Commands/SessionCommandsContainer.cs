using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using ActivesAccounting.Core.Utils;
using ActivesAccounting.Session.Contracts;

using Microsoft.Toolkit.Mvvm.Input;

namespace ActivesAccounting.Commands;

internal sealed class SessionCommandsContainer
{
    private readonly LinkedList<RelayCommand> _relayCommands = new();
    private readonly LinkedList<AsyncRelayCommand> _asyncRelayCommands = new();

    private readonly IAppSession _appSession;

    public SessionCommandsContainer(IAppSession aAppSession) =>
        _appSession = aAppSession;

    public ICommand CreateCommand(Action aAction)
    {
        var command = new RelayCommand(aAction, isSessionOpen);
        _relayCommands.AddLast(command);

        return command;
    }

    public ICommand CreateAsyncCommand(Func<Task> aAction)
    {
        var command = new AsyncRelayCommand(aAction, isSessionOpen);
        _asyncRelayCommands.AddLast(command);

        return command;
    }

    public void NotifySessionChanging()
    {
        _relayCommands.ForEach(aC => aC.NotifyCanExecuteChanged());
        _asyncRelayCommands.ForEach(aC => aC.NotifyCanExecuteChanged());
    }

    private bool isSessionOpen() => _appSession.IsSessionOpen;
}