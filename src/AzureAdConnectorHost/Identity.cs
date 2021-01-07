// -----------------------------------------------------------------------
// <copyright file="Identity.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    /// <summary>
    /// Identity type of request body
    /// </summary>
    public class Identity
    {
        /// <summary>
        /// Gets or sets identity sign in type
        /// </summary>
        public string? SignInType { get; set; }

        /// <summary>
        /// Gets or sets identity issuer assigned id
        /// </summary>
        public string? IssuerAssignedId { get; set; }

        /// <summary>
        /// Gets or sets identity issuer
        /// </summary>
        public string? Issuer { get; set; }
    }
}
