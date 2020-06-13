using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Maktoob.SPA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                CreateHostBuilder(args).Build().Run();

            }
            catch
            {
                CreateHostBuilderForLocalIIS(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilderForLocalIIS(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((hostBuilderContext, configurationBinder) =>
               {
                   if (!hostBuilderContext.HostingEnvironment.IsDevelopment())
                   {
                       var keyVaultEndpoint = GetKeyVaultEndpoint();
                       if (!string.IsNullOrEmpty(keyVaultEndpoint))
                       {
                           var azureServiceTokenProvider = new AzureServiceTokenProvider();
                           var keyVaultClient = new KeyVaultClient(
                               new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback)
                            );

                           configurationBinder.AddAzureKeyVault(
                               keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager()
                            );
                       }
                   }
               })
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });

        private static string GetKeyVaultEndpoint()
            => "https://Maktoobb-kv.vault.azure.net";
    }
}
