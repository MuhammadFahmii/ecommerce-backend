// ------------------------------------------------------------------------------------
// MyControllerProcessor.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using netca.Api.Controllers;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace netca.Api.Processors;

/// <summary>
/// MyControllerProcessor
/// </summary>
public class MyControllerProcessor : IOperationProcessor
{
    /// <summary>
    /// Process
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public bool Process(OperationProcessorContext context)
    {
        return context.ControllerType != typeof(DevelopmentController);
    }
}