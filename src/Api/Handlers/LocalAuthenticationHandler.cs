// ------------------------------------------------------------------------------------
// LocalAuthenticationHandler.cs  2021
// Copyright Ahmad Ilman Fadilah. All rights reserved.
// ahmadilmanfadilah@gmail.com,ahmadilmanfadilah@outlook.com
// -----------------------------------------------------------------------------------

using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace netca.Api.Handlers
{
    /// <summary>
    /// LocalAuthenticationHandler
    /// </summary>
    public class LocalAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        /// <summary>
        /// The default user id, injected into the claims for all requests.
        /// </summary>
        public static readonly Guid UserId = Guid.NewGuid();

        /// <summary>
        /// The name of the authorizaton scheme that this handler will respond to.
        /// </summary>
        public const string AuthScheme = "LocalAuth";

        private readonly Claim DefaultUserIdClaim = new(
            ClaimTypes.NameIdentifier, UserId.ToString());

        /// <summary>
        /// LocalAuthenticationHandler
        /// </summary>
        public LocalAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, 
            UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Marks all authentication requests as successful, and injects the
        /// default company id into the user claims.
        /// </summary>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsIdentity(new[] { DefaultUserIdClaim }, AuthScheme)),
                new AuthenticationProperties(),
                AuthScheme);
            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
