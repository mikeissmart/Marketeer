using Marketeer.Core.Domain.Dtos.Auth;
using Marketeer.Core.Domain.Entities.Market;

namespace Marketeer.Core.Domain.Dtos
{
    public abstract class EntityDto : IMapFrom<HistoryData>
    {
        public int Id { get; set; }
    }

    #region SingleAudit

    public abstract class EntityAuditCreateDto : EntityDto
    {
        public DateTime CreatedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateDto : EntityAuditCreateDto
    {
        public int CreatedByAppUserId { get; set; }
        public AppUserDto CreatedBy { get; set; }
    }

    public abstract class EntityAuditUpdateDto : EntityDto
    {
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class EntityUserAuditUpdateDto : EntityAuditUpdateDto
    {
        public int? UpdatedByAppUserId { get; set; }
        public AppUserDto? UpdatedBy { get; set; }
    }

    public abstract class EntityAuditRemoveDto : EntityDto
    {
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditRemoveDto : EntityAuditRemoveDto
    {
        public int? RemovedByAppUserId { get; set; }
        public AppUserDto? RemovedBy { get; set; }
    }

    #endregion

    #region DoubleAudit

    public abstract class EntityAuditCreateUpdateDto : EntityDto
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateUpdateDto : EntityAuditCreateUpdateDto
    {
        public int CreatedByAppUserId { get; set; }
        public int? UpdatedByAppUserId { get; set; }
        public AppUserDto CreatedBy { get; set; }
        public AppUserDto? UpdatedBy { get; set; }
    }

    public abstract class EntityAuditCreateRemoveDto : EntityDto
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateRemoveDto : EntityAuditCreateRemoveDto
    {
        public int CreatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        public AppUserDto CreatedBy { get; set; }
        public AppUserDto? RemovedBy { get; set; }
    }

    public abstract class EntityAuditUpdateRemoveDto : EntityDto
    {
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditUpdateRemoveDto : EntityAuditUpdateRemoveDto
    {
        public int? UpdatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        public AppUserDto? UpdatedBy { get; set; }
        public AppUserDto? RemovedBy { get; set; }
    }

    #endregion

    #region TripleAudit

    public abstract class EntityAuditCreateUpdateRemoveDto : EntityDto
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? RemovedDate { get; set; }
    }

    public abstract class EntityUserAuditCreateUpdateRemoveDto : EntityAuditCreateUpdateRemoveDto
    {
        public int CreatedByAppUserId { get; set; }
        public int? UpdatedByAppUserId { get; set; }
        public int? RemovedByAppUserId { get; set; }
        public AppUserDto CreatedBy { get; set; }
        public AppUserDto? UpdatedBy { get; set; }
        public AppUserDto? RemovedBy { get; set; }
    }

    #endregion
}
