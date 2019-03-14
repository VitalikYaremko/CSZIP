using Microsoft.Extensions.DependencyInjection;

namespace CSZIP.Web.Site.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddModule<T>(this IServiceCollection services) where T : class, IDependencyModule, new()
        {
            new T().ConfigureServices(services);
        }
    }
}
