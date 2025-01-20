
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;


namespace GitLabServicesLayer
{
	public static class ServicesExtensions
	{

		public static IServiceCollection AddGitLabLayerServices(this IServiceCollection services)
		{
			services.AddTransient<IGitLabServices, GitLabServices>();
			services.AddHttpClient(Options.DefaultName, c =>
			{
#if NET462
				ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
#endif
			})
				.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
				{
#if !NET462
						ClientCertificateOptions = ClientCertificateOption.Manual,
						ServerCertificateCustomValidationCallback =
							(httpRequestMessage, cert, certChain, policyErrors) => true
#endif
					
				});
			return services ; 
		}

		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			// Custom validation logic for .NET Framework 4.6.2
			return true; // Accept all certificates (not recommended for production)
		}




	}
	
}

