namespace ProductsArchive.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    public void ForceSet_Created(DateTime created) => Created = created;
    public void ForceSet_CreatedBy(string createdBy) => CreatedBy = createdBy;
    public void ForceSet_LastModified(DateTime lastModified) => LastModified = lastModified;
    public void ForceSet_LastModifiedBy(string lastModifiedBy) => LastModifiedBy = lastModifiedBy;
}