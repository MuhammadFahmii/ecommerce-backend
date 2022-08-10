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
    public string? CreatedBy { get; set; }
    public long? UpdatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}