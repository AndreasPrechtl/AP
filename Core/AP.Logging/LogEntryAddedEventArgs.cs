namespace AP.Logging;

public class LogEntryAddedEventArgs
{
    public LogEntry Entry { get; private set; }

    public LogEntryAddedEventArgs(LogEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        Entry = entry;
    }
}
