// ------------------------------------------------------------------------------------
// Changelog.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;

namespace netca.Domain.Entities
{
    /// <summary>
    /// Changelog
    /// </summary>
    public class Changelog
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; }

        /// <summary>
        /// Method
        /// </summary>
        /// <value></value>
        public string Method { get; set; }

        /// <summary>
        /// TableName
        /// </summary>
        /// <value></value>
        public string TableName { get; set; }

        /// <summary>
        /// TableName
        /// </summary>
        /// <value></value>
        public string KeyValues { get; set; }

        /// <summary>
        /// NewValues
        /// </summary>
        /// <value></value>
        public string NewValues { get; set; }

        /// <summary>
        /// OldValues
        /// </summary>
        /// <value></value>
        public string OldValues { get; set; }

        /// <summary>
        /// ChangeDate
        /// </summary>
        /// <value></value>
        public DateTime ChangeDate { get; set; }
    }
}
