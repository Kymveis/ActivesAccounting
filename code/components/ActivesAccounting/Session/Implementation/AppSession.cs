using System;
using System.IO;

using ActivesAccounting.Core.Model.Contracts;
using ActivesAccounting.Core.Utils;
using ActivesAccounting.Session.Contracts;

namespace ActivesAccounting.Session.Implementation
{
    internal sealed class AppSession : IAppSession
    {
        private ISession? _actualSession;

        public bool IsSessionOpen => _actualSession is not null;

        public ISession ActualSession
        {
            get => _actualSession ?? throw new InvalidOperationException("Session is not open.");
            set => _actualSession = value;
        }

        public FileInfo? File { get; set; }
    }
}