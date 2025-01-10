using System.Threading;
using System.Threading.Tasks;

namespace AP.Data;

public interface IPersist : AP.IContextDependentDisposable
{
    Task Save(CancellationToken cancellation = default);
    void Discard();
}
