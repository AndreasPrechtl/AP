using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP.Linq;

namespace AP.Logging
{
    public class LogEntryAddedEventArgs
    {
        public LogEntry Entry { get; private set; }

        public LogEntryAddedEventArgs(LogEntry entry)
        {
            ExceptionHelper.AssertNotNull(() => entry);
            Entry = entry;
        }
    }
}
