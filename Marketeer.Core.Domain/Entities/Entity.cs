using Marketeer.Core.Domain.Entities.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketeer.Core.Domain
{
    public abstract class Entity
    {
        public int Id { get; set; }
    }

    #region SingleAudit

    public abstract class EntityAuditCreate : Entity
    {
        public DateTime CreatedDate { get; set; }
    }

    public abstract class EntityUserAuditCreate : EntityAuditCreate
    {
        public int CreatedByAppUserId { get; set; }
        [ForeignKey("CreatedByAppUserId")]
        public AppUser CreatedBy { get; set; }
    }

    public abstract class EntityAuditUpdate : Entity
    {
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class EntityUserAuditUpdate : EntityAuditUpdate
    {
        public int? UpdatedByAppUserId { get; set; }
        [ForeignKey("UpdatedByAppUserId")]
        public AppUser? UpdatedBy { get; set; }
    }

    public abstract class EntityAuditRemove : Entity
    {
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditRemove : EntityAuditRemove
    {
        public int? RemovedByAppUserId { get; set; }
        [ForeignKey("RemovedByAppUserId")]
        public AppUser? RemovedBy { get; set; }
    }

    #endregion

    #region DoubleAudit

    public abstract class EntityAuditCreateUpdate : Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateUpdate : EntityAuditCreateUpdate
    {
        public int CreatedByAppUserId { get; set; }
        public int? UpdatedByAppUserId { get; set; }
        [ForeignKey("CreatedByAppUserId")]
        public AppUser CreatedBy { get; set; }
        [ForeignKey("UpdatedByAppUserId")]
        public AppUser? UpdatedBy { get; set; }
    }

    public abstract class EntityAuditCreateRemove : Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateRemove : EntityAuditCreateRemove
    {
        public int CreatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        [ForeignKey("CreatedByAppUserId")]
        public AppUser CreatedBy { get; set; }
        [ForeignKey("RemovedByAppUserId")]
        public AppUser? RemovedBy { get; set; }
    }

    public abstract class EntityAuditUpdateRemove : Entity
    {
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditUpdateRemove : EntityAuditUpdateRemove
    {
        public int? UpdatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        [ForeignKey("UpdatedByAppUserId")]
        public AppUser? UpdatedBy { get; set; }
        [ForeignKey("RemovedByAppUserId")]
        public AppUser? RemovedBy { get; set; }
    }

    #endregion

    #region TripleAudit

    public abstract class EntityAuditCreateUpdateRemove : Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateUpdateRemove : EntityAuditCreateUpdateRemove
    {
        public int CreatedByAppUserId { get; set; }
        public int? UpdatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        [ForeignKey("CreatedByAppUserId")]
        public AppUser CreatedBy { get; set; }
        [ForeignKey("UpdatedByAppUserId")]
        public AppUser? UpdatedBy { get; set; }
        [ForeignKey("RemovedByAppUserId")]
        public AppUser? RemovedBy { get; set; }
    }

    #endregion
}
