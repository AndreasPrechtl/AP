using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Logging
{
    public static class Logger
    {
        public static void WriteToLog(LogEntry entry) { _writer.Write(entry); }
        public static IEnumerable<LogEntry> ReadLog() { return _reader.Read(); }

        public static readonly object GlobalLock = new object();
        
        private static LogWriterBase _writer;
        private static LogReaderBase _reader;

        /// <summary>
        /// Fired when the log is updated - relayed - originally listening to the writer's LogUpdated event
        /// </summary>
        public static event LogEntryAddedEventHandler LogUpdated
        {
            add { _writer.LogEntryAdded += value; }
            remove { _writer.LogEntryAdded -= value; }
        }
        
        /// <summary>
        /// Todo: improve initialization / cleanup...
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="reader"></param>
        public static void Initialize(LogWriterBase writer, LogReaderBase reader)
        {
            _writer = writer;
            _reader = reader;
        }
    }
}
