// -----------------------------------------------------------------------
// <copyright file="ResponseContent.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    /// <summary>
    /// Definition of Response Content class for AD B2C Response
    /// </summary>
     public class ResponseContent
    {
        /// <summary>
        /// The API Version os the request
        /// </summary>
        public const string ApiVersion = "0.0.1";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseContent"/> class.
        /// </summary>
        /// <param name="action">The action to take. Defaults to continue</param>
        /// <param name="userMessage">The message to display to the user</param>
        /// <param name="status">The status to display to the user</param>
        public ResponseContent(string action = "Continue", string? userMessage = null, string status = "")
        {
            this.Action = action;
            this.UserMessage = userMessage;

            if (status == "400")
            {
                this.Status = "400";
            }
        }

        /// <summary>
        /// Gets the Version of this API
        /// </summary>
        public string Version { get; } = ResponseContent.ApiVersion;

        /// <summary>
        /// Gets or sets the Action to Take
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the message to display to the user
        /// </summary>
        public string? UserMessage { get; set; }

        /// <summary>
        /// Gets or sets the status of this response
        /// </summary>
        public string? Status { get; set; }
    }
}
