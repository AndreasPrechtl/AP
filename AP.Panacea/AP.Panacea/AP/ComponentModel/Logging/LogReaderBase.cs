using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AP.ComponentModel.Logging
{
    public abstract class LogSyncronizationContextUserBase
    {
        public event LogEntryAddedEventHandler LogEntryAdded;

        /// <summary>
        /// Contains a reference to the underlying syncronizationContext
        /// </summary>
        protected readonly object SyncRoot;

        private LogSyncronizationContext _syncronizationContext;
        public LogSyncronizationContext SyncronizationContext { get { return _syncronizationContext; } }

        internal LogSyncronizationContextUserBase(LogSyncronizationContext syncronizationContext = null)
        {
            _syncronizationContext = syncronizationContext = syncronizationContext ?? new LogSyncronizationContext();            
            this.SyncRoot = syncronizationContext.SyncRoot;
        }

        /// <summary>
        /// Raises the LogEntryAdded event
        /// </summary>
        /// <param name="entry"></param>
        protected virtual void OnLogEntryAdded(LogEntry entry)
        {
            LogEntryAddedEventHandler handler = this.LogEntryAdded;

            if (handler != null)
                handler(this, new LogEntryAddedEventArgs(entry));
        }
    }

    public abstract class LogReaderBase : LogSyncronizationContextUserBase
    {
        protected LogReaderBase(LogSyncronizationContext syncronizationContext = null)
            : base(syncronizationContext)
        { }
     
        public abstract IEnumerable<LogEntry> Read();
    }

    public class StreamingLogReader: LogReaderBase
    {
        private readonly Activator<Stream> _streamCreator;
        private BufferedLogSyncronizationContext Buffer { get { return (BufferedLogSyncronizationContext)base.SyncronizationContext; } }

        public StreamingLogReader(Activator<Stream> streamCreator, BufferedLogSyncronizationContext buffer = null)
            : base(buffer ?? new BufferedLogSyncronizationContext(100))
        {
            if (streamCreator == null)
                throw new ArgumentNullException("streamCreator");

            _streamCreator = streamCreator;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override IEnumerable<LogEntry> Read()
        {
            // ok here's what i need to do - read the buffer - inverted order, then the file also inverted? i donno yet...
            // todo: make it async .. ?? nouh?
            
            // read everything the buffer has to offer
            LogEntry[] snapshot = null;
            lock (SyncRoot)
                this.Buffer.GetSnapshot(out snapshot);

            List<LogEntry> entries = new List<LogEntry>(snapshot);

            const string begin = "<!-- entry begin -->";
            lock (SyncRoot)
            {
                Stream stream = null;

                try
                {
                    stream = _streamCreator();
                    
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        StringBuilder sb = new StringBuilder();

                        string current = reader.ReadLine();

                        while (current != null)
                        {
                            if (current == begin)
                            {
                                current = reader.ReadLine();

                                while (current != begin)
                                {
                                    sb.Append(current);
                                    current = reader.ReadLine();
                                }
                                entries.Add(Serialization.Deserialize.Xaml<LogEntry>(sb.ToString()));
                                sb.Clear();
                            }
                        }
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }
            }
            return entries;
        }
    }
}
