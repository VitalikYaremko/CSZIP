using CSZIP.Web.Site.Domain.Interfaces;
using CSZIP.Web.Site.Domain.Managers;
using CSZIP.Web.Site.Infrastructure.DependencyInjection;
using CSZIP.Web.Site.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSZIP.Web.Site.Infrastructure.Modules
{
    public class ServicesModule : IDependencyModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFileProcessingService, FileProcessingService>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
        }
    }
}
