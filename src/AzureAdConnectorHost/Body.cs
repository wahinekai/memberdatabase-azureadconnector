// -----------------------------------------------------------------------
// <copyright file="Body.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// The required/relevant JSON fields from the body of a request
    /// </summary>
    public class Body : IValidatable
    {
        /// <summary>
        /// Gets or sets the email from the body
        /// </summary>
        public string? Email { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            this.Email = Ensure.IsNotNullOrWhitespace(() => this.Email);
        }
    }
}
