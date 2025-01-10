using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AP.Logging;

public abstract class LogSynchronizationContextUserBase
{
    public event LogEntryAddedEventHandler LogEntryAdded;

    /// <summary>
    /// Contains a reference to the underlying syncronizationContext
    /// </summary>
    protected readonly object SyncRoot;

    private LogSynchronizationContext _syncronizationContext;
    public LogSynchronizationContext SyncronizationContext => _syncronizationContext;

    internal LogSynchronizationContextUserBase(LogSynchronizationContext? syncronizationContext = null)
    {
        _syncronizationContext = syncronizationContext = syncronizationContext ?? new LogSynchronizationContext();
        SyncRoot = syncronizationContext.SyncRoot;
    }

    /// <summary>
    /// Raises the LogEntryAdded event
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void OnLogEntryAdded(LogEntry entry)
    {
        LogEntryAddedEventHandler handler = LogEntryAdded;

        if (handler != null)
            handler(this, new LogEntryAddedEventArgs(entry));
    }
}

public abstract class LogReaderBase : LogSynchronizationContextUserBase
{
    protected LogReaderBase(LogSynchronizationContext? syncronizationContext = null)
        : base(syncronizationContext)
    { }

    public abstract IEnumerable<LogEntry> Read();
}

public class StreamingLogReader : LogReaderBase
{
    private readonly Activator<Stream> _streamCreator;
    private BufferedLogSynchronizationContext Buffer => (BufferedLogSynchronizationContext)SyncronizationContext;

    public StreamingLogReader(Activator<Stream> streamCreator, BufferedLogSynchronizationContext? buffer = null)
        : base(buffer ?? new BufferedLogSynchronizationContext(100))
    {
        ArgumentNullException.ThrowIfNull(streamCreator);

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
            Buffer.GetSnapshot(out snapshot);

        List<LogEntry> entries = new(snapshot);

        const string begin = "<!-- entry begin -->";
        lock (SyncRoot)
        {
            Stream stream = null!;

            try
            {
                stream = _streamCreator();

                using (StreamReader reader = new(stream))
                {
                    StringBuilder sb = new();

                    string current = reader.ReadLine()!;

                    while (current != null)
                    {
                        if (current == begin)
                        {
                            current = reader.ReadLine()!;

                            while (current != begin)
                            {
                                sb.Append(current);
                                current = reader.ReadLine()!;
                            }
                            entries.Add(JsonSerializer.Deserialize<LogEntry>(sb.ToString())!);
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
