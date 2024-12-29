using AP.Data;

namespace AP.Security
{
    internal interface IContextPartInternal<out T> : IQuery<T>
        where T : class
    {
        string GetDescription(string name);
        void SetDescription(string name, string description);

        void Rename(string name, string newName);
        void Delete(string name);

        bool Exists(string name);
    }
}
