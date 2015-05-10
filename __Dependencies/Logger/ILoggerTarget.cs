using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gritsenko
{
    public interface ILoggerTarget
    {
        bool EventsOnly { get; }

        void OnLogged(LogEntry logEntry);
    }
}
