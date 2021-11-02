// ------------------------------------------------------------------------------------
// SendToMsTeam.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Text;
using netca.Application.Common.Exceptions;
using netca.Application.Common.Models;
using Serilog;

namespace netca.Infrastructure.Apis
{
    /// <summary>
    /// SendToMsTeam
    /// </summary>
    public static class SendToMsTeam
    {
        /// <summary>
        /// SendToMsTeam
        /// </summary>
        /// <param name="appSetting"></param>
        /// <param name="tmpl"></param>
        public static void Send(AppSetting appSetting, MsTeamTemplate tmpl)
        {
            Retry.Do(SendTo, appSetting, tmpl, TimeSpan.FromSeconds(5));
        }

        private static void SendTo(AppSetting appSetting, MsTeamTemplate tmpl)
        {
            var logger = Log.ForContext(typeof(SendToMsTeam));
            try
            {
                using var client = new HttpClient(new HttpHandler(new HttpClientHandler()));
                client.DefaultRequestHeaders.Add(appSetting.Bot.Header, appSetting.Bot.Secret);
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(tmpl), Encoding.UTF8, Constants.HeaderJson);
                var response = client.PostAsync(new Uri(appSetting.Bot.Address), content).Result;

                logger.Verbose("Response:");
                logger.Verbose(response.ToString());

                if (response.IsSuccessStatusCode)
                    return;

                throw new ThrowException("Request Failed");
            }
            catch (Exception e)
            {
                logger.Fatal($"failed send to Microsoft team {e.Message}");
            }
        }
    }
}