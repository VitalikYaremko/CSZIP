
using CSZIP.Web.Site.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace CSZIP.Web.Site.Infrastructure.Modules
{
    public class DependencyAggregateModule : IDependencyModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModule<ServicesModule>(); 
        }
    }
}
