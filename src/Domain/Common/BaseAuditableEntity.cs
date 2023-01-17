// ------------------------------------------------------------------------------------
// BaseAuditableEntity.cs  2022
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Domain.Common;

/// <summary>
/// BaseAuditableEntity
/// </summary>
public abstract record BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets createdBy
    /// </summary>
    /// <value></value>
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets createdDate
    /// </summary>
    /// <value></value>
    public long CreatedDate { get; set; }

    /// <summary>
    /// Gets or sets updatedBy
    /// </summary>
    /// <value></value>
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets updatedDate
    /// </summary>
    /// <value></value>
    public long? UpdatedDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether isDeleted
    /// </summary>
    /// <value></value>
    public bool IsDeleted { get; set; }
}
