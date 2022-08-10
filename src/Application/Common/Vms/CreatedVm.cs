// ------------------------------------------------------------------------------------
// CreatedVm.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

namespace netca.Application.Common.Vms;

/// <summary>
/// CreatedVm
/// </summary>
public class CreatedVm
{
    /// <summary>
    /// Gets or sets id
    /// </summary>
    /// <value></value>
    public Guid Id { get; set; } = Guid.NewGuid();
}