// ------------------------------------------------------------------------------------
// LogEventSinkHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using netca.Application.Common.Models;
using Serilog.Core;
using Serilog.Events;

namespace netca.Api.Handlers
{
    /// <summary>
    /// LogEventSinkHanlder
    /// </summary>
    public class LogEventSinkHandler : ILogEventSink
    {
        private readonly AppSetting _appSetting;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogEventSinkHandler"/> class.
        /// </summary>
        /// <param name="appSetting"></param>
        public LogEventSinkHandler(AppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        /// <summary>
        /// Emit
        /// </summary>
        /// <param name="logEvent"></param>
        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level.ToString().Equals("Error"))
            {
                Console.WriteLine("xxx" + _appSetting.App.Description);
            }
        }
    }
}