using System;
using System.IO;
using System.Threading.Tasks;

namespace AP.Logging;

public abstract class LogWriterBase : LogSyncronizationContextUserBase
{
    protected LogWriterBase(LogSyncronizationContext? syncronizationContext = null)
        : base(syncronizationContext)
    { }

    public void Write(LogEntry entry)
    {
        OnWrite(entry);
        OnLogEntryAdded(entry);
    }

    protected abstract void OnWrite(LogEntry entry);
}

public class StreamingLogWriter : LogWriterBase
{
    private readonly Activator<Stream> _streamCreator;
    private BufferedLogSyncronizationContext Buffer => (BufferedLogSyncronizationContext)SyncronizationContext;

    public StreamingLogWriter(Activator<Stream> streamCreator, BufferedLogSyncronizationContext? buffer = null)
        : base(buffer ?? new BufferedLogSyncronizationContext(100))
    {
        ArgumentNullException.ThrowIfNull(streamCreator);

        _streamCreator = streamCreator;
    }

    protected override void OnWrite(LogEntry entry)
    {
        bool isBufferFilled = false;

        lock (SyncRoot)
        {
            Buffer.Add(entry);
            isBufferFilled = Buffer.IsFilled;
        }

        if (isBufferFilled)
            Flush();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
    protected virtual void Flush()
    {
        LogEntry[] entries = null;

        lock (SyncRoot)
        {
            // use the buffer to get a copy and clear it afterwards
            Buffer.GetSnapshot(out entries);
            Buffer.Clear();
        }

        // make writing thread safe and start it in an extra task
        // the problem however - I cannot read items off the writer queue
        // meaning: as long as it isn't flushed the reader can't read the newly added objects
        // I should probably use the IProducerConsumerCollection interface?
        Task task = new            (
            delegate ()
            {
                const string begin = "<!-- entry begin -->";

                // I probably won't need most of those locks / writers
                lock (SyncRoot)
                {
                    Stream stream = null;

                    try
                    {
                        stream = _streamCreator();

                        using (StreamWriter writer = new(stream))
                        {
                            foreach (LogEntry entry in entries)
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                writer.WriteLine(begin);
                                writer.Write(Serialization.Serialize.Xaml(entry));
                            }

                            stream = null;
                        }
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Dispose();

                    }
                }
            }
        );

        task.Start();
    }
}
