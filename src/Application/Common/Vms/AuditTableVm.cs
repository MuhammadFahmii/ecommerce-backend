// ------------------------------------------------------------------------------------
// AuditTableVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Application.Common.Vms
{
    /// <summary>
    /// AuditTableVm
    /// </summary>
    public abstract class AuditTableVm
    {
        /// <summary>
        /// Gets or sets id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets createdBy
        /// </summary>
        /// <value></value>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets createdDate
        /// </summary>
        /// <value></value>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets updatedBy
        /// </summary>
        /// <value></value>
        public string? UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets updatedDate
        /// </summary>
        /// <value></value>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets deletedDate
        /// </summary>
        /// <value></value>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether isActive
        /// </summary>
        /// <value></value>
        public bool IsActive { get; set; }
    }
}
