using System;

namespace AP.Configuration;

/// <summary>
/// Base class for ContentProviders using file access.
/// </summary>
/// <typeparam name="TContent">The content type.</typeparam>
public abstract class FileContentProviderBase<TContent> : ProviderBase, IContentProvider<TContent>, IFileBasedProvider
{
    private readonly string _fileName;
    private DateTimeOffset? _lastReadDate;
    private TContent _content;

    public TContent Content
    {
        get
        {
            if (_lastReadDate.HasValue)
            {
                DateTimeOffset dt = AP.IO.FileSystem.Context.Get(_fileName).DateModified;

                if (_lastReadDate.Value > dt)
                    return _content;
            }

            try
            {
                TContent c = this.ReadContent();
                _content = c;
                _lastReadDate = DateTimeOffset.UtcNow;
                
                return c;                
            }
            catch (Exception e)
            {
                throw e;
            }                  
        }
    }

    protected FileContentProviderBase(string? name = null)
        : base(name)
    { }

    protected FileContentProviderBase(string fileName, string? name = null)
        : base(name)
    {
        ArgumentNullException.ThrowIfNull(fileName);

        _fileName = fileName;            
    }

    protected abstract TContent ReadContent();

    #region IFileBasedProvider Members

    public string FileName
    {
        get { return _fileName; }
    }
    
    public DateTimeOffset? LastReadDate
    {
        get { return _lastReadDate; }
    }

    #endregion
}
