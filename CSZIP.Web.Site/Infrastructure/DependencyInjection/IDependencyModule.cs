using Microsoft.Extensions.DependencyInjection;

namespace CSZIP.Web.Site.Infrastructure.DependencyInjection
{
    public interface IDependencyModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
