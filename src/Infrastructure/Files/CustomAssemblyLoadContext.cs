// ------------------------------------------------------------------------------------
// CustomAssemblyLoadContext.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Reflection;
using System.Runtime.Loader;

namespace netca.Infrastructure.Files;

/// <summary>
/// CustomAssemblyLoadContext
/// </summary>
public class CustomAssemblyLoadContext : AssemblyLoadContext
{
    /// <summary>
    /// LoadUnmanagedLibrary
    /// </summary>
    /// <param name="absolutePath"></param>
    /// <returns></returns>
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
    {
        return LoadUnmanagedDll(absolutePath);
    }

    /// <summary>
    /// LoadUnmanagedDll
    /// </summary>
    /// <param name="unmanagedDllName"></param>
    /// <returns></returns>
    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        return LoadUnmanagedDllFromPath(unmanagedDllName);
    }

    /// <summary>
    /// Load
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    protected override Assembly Load(AssemblyName assemblyName)
    {
        throw new NotImplementedException();
    }
}