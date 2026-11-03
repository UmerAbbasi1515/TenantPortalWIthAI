using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UBSAIAgent;

namespace AIServices
{
    public static class AIServiceExtensions
    {
        public static IServiceCollection AddAIService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSemanticViewBaseAIAgent(configuration);

            services.AddScoped<IApplicationAIAssistant, ApplicationAIAssistant>();

            return services;
        }
    }
}
