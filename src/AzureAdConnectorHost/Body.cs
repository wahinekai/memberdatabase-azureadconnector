// -----------------------------------------------------------------------
// <copyright file="Body.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    using System.Collections.Generic;

    /// <summary>
    /// Body of Authentication Request that we care about
    /// </summary>
    public class Body
    {
        /// <summary>
        /// Gets or sets body identities
        /// </summary>
        public ICollection<Identity> Identities { get; set; } = new List<Identity>();
    }
}
