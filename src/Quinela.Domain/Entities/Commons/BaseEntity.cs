namespace Quinela.Domain.Entities.Commons
{
    /// <summary>
    /// Base comun para todas las entidades (copiada de Nemesis): id + campos de auditoria.
    /// </summary>
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string CreatedBy { get; set; } = string.Empty;
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual string? UpdatedBy { get; set; }
        public virtual bool Active { get; set; }
    }
}
