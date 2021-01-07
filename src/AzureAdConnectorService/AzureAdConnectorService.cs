// -----------------------------------------------------------------------
// <copyright file="AzureAdConnectorService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Service
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using WahineKai.Common;
    using WahineKai.Common.Api.Services;
    using WahineKai.MemberDatabase.AzureAdConnector.Service.Contracts;
    using WahineKai.MemberDatabase.Dto;
    using WahineKai.MemberDatabase.Dto.Contracts;
    using WahineKai.MemberDatabase.Dto.Models;
    using WahineKai.MemberDatabase.Dto.Properties;

    /// <summary>
    /// Implementation of API Connector Service
    /// </summary>
    public class AzureAdConnectorService : ServiceBase, IAzureAdConnectorService
    {
        private readonly IUserRepository<AdminUser> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAdConnectorService"/> class.
        /// </summary>
        /// <param name="configurationMaybeNull">Application configuration</param>
        /// <param name="loggerFactory">Logger factory given by ASP.NET</param>
        public AzureAdConnectorService(IConfiguration? configurationMaybeNull, ILoggerFactory loggerFactory)
            : base(loggerFactory, configurationMaybeNull)
        {
            // Validate input arguments
            var configuration = Ensure.IsNotNull(() => configurationMaybeNull);

            var cosmosConfiguration = CosmosConfiguration.BuildFromConfiguration(configuration);
            this.repository = new CosmosUserRepository<AdminUser>(cosmosConfiguration, loggerFactory);
        }

        /// <inheritdoc/>
        public async Task<bool> CanSignUpFromEmail(string? emailMaybeNull)
        {
            // Verify input arguments
            var email = Ensure.IsNotNullOrWhitespace(() => emailMaybeNull);

            this.Logger.LogInformation($"Checking to see whether user with email {email} is eligible to sign up to the application");

            try
            {
                // Get user from database
                var user = await this.repository.GetUserByEmailAsync(email);

                // Ensure user is active administrator
                Ensure.IsTrue(() => user.Active);
                Ensure.IsTrue(() => user.Admin);

                // User is eligible to sign up for application
                this.Logger.LogInformation($"Valid user {email} was approved for application login.");
                return true;
            }
            catch (Exception)
            {
                // If try code throws, then user is not eligible to sign up for application
                this.Logger.LogWarning($"Invalid user {email} tried to sign up for application. {email} was blocked");
                return false;
            }
        }
    }
}
