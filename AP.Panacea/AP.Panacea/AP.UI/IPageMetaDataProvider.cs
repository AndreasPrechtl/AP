using AP.UniversalIdentifiers;

namespace AP.UI
{
    public interface IPageMetaDataProvider 
    {
        PageMetaData GetMetaData(object id);
    }
}