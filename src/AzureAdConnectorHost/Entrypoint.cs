// -----------------------------------------------------------------------
// <copyright file="Entrypoint.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.AzureAdConnector.Host
{
    using System;
    using Azure.Extensions.AspNetCore.Configuration.Secrets;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Entrypoint to Backend API
    /// </summary>
    public sealed class Entrypoint : IEntrypoint
    {
        private readonly string[] commandLineArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entrypoint"/> class.
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        public Entrypoint(string[] args)
        {
            this.commandLineArguments = args;
        }

        /// <summary>
        /// Main method entrypoint
        /// </summary>
        /// <param name="args">Command line arguments</param>
        public static void Main(string[] args)
        {
            var program = new Entrypoint(args);
            program.Start();
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.CreateHostBuilder(this.commandLineArguments).Build().Run();
        }

        /// <summary>
        /// Creates a host builder
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>A Host builder that can be built and run</returns>
        public IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();

                    var env = hostingContext.HostingEnvironment;

                    // Add configuration files
                    config.AddJsonFile("Properties/appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"Properties/{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    config.AddEnvironmentVariables();

                    // Add secrets
                    if (env.IsDevelopment())
                    {
                        config.AddUserSecrets<Entrypoint>(optional: true, reloadOnChange: true);
                    }
                    else
                    {
                        var builtConfig = config.Build();
                        var secretClient = new SecretClient(new Uri(builtConfig["KeyVault:Url"]), new DefaultAzureCredential());
                        config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
                    }

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
