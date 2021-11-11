// ------------------------------------------------------------------------------------
// SendToMsTeams.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using netca.Application.Common.Models;
using Serilog;

namespace netca.Infrastructure.Apis
{
    /// <summary>
    /// SendToMsTeams
    /// </summary>
    public static class SendToMsTeams
    {
        private static readonly ILogger Logger = Log.ForContext(typeof(SendToMsTeams));

        /// <summary>
        /// SendToMsTeam
        /// </summary>
        /// <param name="appSetting"></param>
        /// <param name="tmpl"></param>
        /// <returns></returns>
        public static async Task Send(AppSetting appSetting, MsTeamTemplate tmpl)
        {
            using var client = new HttpClient(new HttpHandler(new HttpClientHandler()));
            client.DefaultRequestHeaders.Add(appSetting.Bot.Header, appSetting.Bot.Secret);
            var str = Newtonsoft.Json.JsonConvert.SerializeObject(tmpl);
            var count = Encoding.UTF8.GetByteCount(str);
            if (count > 5242880)
            {
                Logger.Warning($"Cancelling send message to teams. Size {count} byte too large");
            }
            else
            {
                var content = new StringContent(str, Encoding.UTF8, Constants.HeaderJson);
                var response = client.PostAsync(new Uri(appSetting.Bot.Address), content);
                await response;
            }
        }
    }
}
