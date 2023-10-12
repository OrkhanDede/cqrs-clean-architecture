using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync(CancellationToken cancellationToken);
        Task CompleteAsync();
        void Complete();

    }
}
