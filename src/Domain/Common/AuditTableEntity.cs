// ------------------------------------------------------------------------------------
// AuditableEntity.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
namespace netca.Domain.Common
{
    /// <summary>
    /// AuditableEntity
    /// </summary>
    public abstract class AuditTableEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>
        /// <value></value>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        /// <value></value>
        public DateTime ? CreatedDate { get; set; }

        /// <summary>
        /// UpdatedBy
        /// </summary>
        /// <value></value>
        public Guid UpdatedBy { get; set; }

        /// <summary>
        /// UpdatedDate
        /// </summary>
        /// <value></value>
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// DeletedDate
        /// </summary>
        /// <value></value>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// IsActive
        /// </summary>
        /// <value></value>
        public bool IsActive { get; set; }
    }
}
