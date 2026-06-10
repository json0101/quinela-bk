using Microsoft.EntityFrameworkCore;
using Quinela.Application.Common.Abstractions;
using Quinela.Infrastructure.Persistence;
using Quinela.Domain.Entities.Commons;

namespace Quinela.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly QuinelaContext _context;
        private readonly DbSet<T> _entities;

        public Repository(QuinelaContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public DbSet<T> GetDbSet() => _entities;
        public IEnumerable<T> GetAll() => _entities.AsEnumerable();
        public T? Get(long id) => _entities.SingleOrDefault(s => s.Id == id);

        public void Insert(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _entities.Add(entity);
        }

        public void Insert(List<T> entity)
        {
            if (entity == null || entity.Count == 0) throw new ArgumentNullException(nameof(entity));
            _entities.AddRange(entity);
        }

        public void Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _entities.Update(entity);
        }

        public void Delete(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _entities.Remove(entity);
        }

        public void Remove(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _entities.Remove(entity);
        }

        public void Detach(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
