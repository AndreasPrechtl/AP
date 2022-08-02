using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AP;
using AP.Linq;

namespace AP.ComponentModel.Logging
{
    public delegate void LogEntryAddedEventHandler(object sender, LogEntryAddedEventArgs e);

    public class LogSyncronizationContext
    {
        public readonly object SyncRoot = new object();
        
        public LogSyncronizationContext()
        { }
    }

    public class BufferedLogSyncronizationContext : LogSyncronizationContext
    {        
        private List<LogEntry> _buffer;
        private int _minimumBufferSize;

        public BufferedLogSyncronizationContext(int minimumBufferSize = 100)
        {
            _buffer = new List<LogEntry>(minimumBufferSize);
            _minimumBufferSize = minimumBufferSize;
        }

        public void Add(LogEntry entry)
        {
            lock (SyncRoot)
                _buffer.Add(entry);
        }

        public int Count
        {
            get 
            { 
                lock (SyncRoot)
                    return _buffer.Count;             
            }
        }

        public void Clear()
        {
            lock (SyncRoot)
                _buffer.Clear();
        }

        public void GetSnapshot(out LogEntry[] entries)
        {
            lock (SyncRoot)
                entries = _buffer.ToArray();            
        }

        public bool IsFilled 
        { 
            get 
            { 
                lock (SyncRoot)
                    return _buffer.Count >= _minimumBufferSize;             
            } 
        }
    }
}
