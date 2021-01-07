// -----------------------------------------------------------------------
// <copyright file="BasicAuthenticationHandler.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    using System;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using WahineKai.MemberDatabase.AzureAdConnector.Host.Properties;
    using WahineKai.Common;

    /// <summary>
    /// Handler for basic authentication
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AuthorizationConfiguration authorizationConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationHandler"/> class.
        /// </summary>
        /// <param name="options">Authentication scheme options</param>
        /// <param name="loggerFactory">Logger factory</param>
        /// <param name="encoder">URL encoder</param>
        /// <param name="clock">System clock</param>
        /// <param name="configuration">App configuration</param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, loggerFactory, encoder, clock)
        {
            // Validate input arugments
            configuration = Ensure.IsNotNull(() => configuration);

            // Build configuration
            this.authorizationConfiguration = new AuthorizationConfiguration(configuration);
        }

        /// <inheritdoc/>
#pragma warning disable CS1998 // Required async to override
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
#pragma warning restore CS1998
        {
            if (!this.Request.Headers.ContainsKey("Authorization"))
            {
                this.Logger.LogWarning("Request blocked with no authorization header");
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                // Check if the HTTP Authorization header exists and read it
                Ensure.IsTrue(() => this.Request.Headers.ContainsKey("Authorization"), "Request contains authorization header");
                var auth = this.Request.Headers["Authorization"].ToString();

                // Ensure the type of the authorization header id `Basic`
                Ensure.IsTrue(() => auth.StartsWith("Basic "), "Authorization header is basic authorization");

                // Get the the HTTP basinc authorization credentials
                var cred = Encoding.UTF8.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var username = cred[0];
                var password = cred[1];

                // Ensure username and password match what we have here
                this.authorizationConfiguration.Authorize(username, password);

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                };

                var identity = new ClaimsIdentity(claims, this.Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

                this.Logger.LogDebug("Basic authentication was sucessfully completed");
                return AuthenticateResult.Success(ticket);
            }
            catch
            {
                this.Logger.LogWarning("Request blocked with malformed authorization header");
                return AuthenticateResult.Fail("Unauthorized");
            }
        }
    }
}
