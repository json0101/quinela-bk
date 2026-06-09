using Quinela.Application.Common.Abstractions;
using Quinela.Infrastructure.Persistence;

namespace Quinela.Infrastructure.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly QuinelaContext _context;
        public UnitOfWork(QuinelaContext context) => _context = context;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);
    }
}
