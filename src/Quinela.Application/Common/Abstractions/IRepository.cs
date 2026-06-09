using Microsoft.EntityFrameworkCore;
using Quinela.Domain.Entities.Commons;

namespace Quinela.Application.Common.Abstractions
{
    /// <summary>Repositorio genérico (mismo contrato que en Nemesis).</summary>
    public interface IRepository<T> where T : BaseEntity
    {
        DbSet<T> GetDbSet();
        IEnumerable<T> GetAll();
        T? Get(long id);
        void Insert(T entity);
        void Insert(List<T> entity);
        void Update(T entity);
        void Delete(T entity);
        void Remove(T entity);
    }
}
