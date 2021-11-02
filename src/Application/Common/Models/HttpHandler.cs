// ------------------------------------------------------------------------------------
// HttpHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace netca.Application.Common.Models
{
    /// <summary>
    /// HttpHandler
    /// </summary>
    public class HttpHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class.
        /// </summary>
        /// <param name="innerHandler"></param>
        public HttpHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        { }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await base.SendAsync(request, cancellationToken);
        }
    }
}