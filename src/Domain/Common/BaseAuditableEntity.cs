// ------------------------------------------------------------------------------------
// BaseAuditableEntity.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Domain.Common;

/// <summary>
/// BaseAuditableEntity
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    public long CreatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public long? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}