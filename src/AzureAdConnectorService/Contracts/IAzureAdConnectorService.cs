// -----------------------------------------------------------------------
// <copyright file="IAzureAdConnectorService.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Service.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for an Api Connector Service
    /// </summary>
    public interface IAzureAdConnectorService
    {
        /// <summary>
        /// Checks a repository to determine whether a user with a specific email can sign up to the application
        /// </summary>
        /// <param name="emailMaybeNull">The email of the user to check</param>
        /// <returns>Whether the user can sign up</returns>
        Task<bool> CanSignUpFromEmail(string? emailMaybeNull);
    }
}
