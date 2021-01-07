// -----------------------------------------------------------------------
// <copyright file="AuthorizationConfiguration.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host.Properties
{
    using Microsoft.Extensions.Configuration;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Authorization Configuration for API Connector Host
    /// </summary>
    public class AuthorizationConfiguration : IValidatable
    {
        /// <summary>
        /// Holds value of username
        /// </summary>
        private readonly string username;

        /// <summary>
        /// Holds value of password
        /// </summary>
        private readonly string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationConfiguration"/> class.
        /// </summary>
        /// <param name="configuration">Dotnet configuration</param>
        public AuthorizationConfiguration(IConfiguration configuration)
        {
            // Sanity check input arguments
            configuration = Ensure.IsNotNull(() => configuration);

            this.username = Ensure.IsNotNullOrWhitespace(() => configuration["BasicAuth:Username"]);
            this.password = Ensure.IsNotNullOrWhitespace(() => configuration["BasicAuth:Password"]);

            this.Validate();
        }

        /// <summary>
        /// Tries to authorize a username and password
        /// </summary>
        /// <param name="username">The username to authorize</param>
        /// <param name="password">The password to authorize</param>
        public void Authorize(string username, string password)
        {
            Ensure.AreEqual(() => username, () => this.username);
            Ensure.AreEqual(() => password, () => this.password);
        }

        /// <inheritdoc/>
        public void Validate()
        {
            Ensure.IsNotNullOrWhitespace(() => this.username);
            Ensure.IsNotNullOrWhitespace(() => this.password);
        }
    }
}
