using CSZIP.API.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Infrastructure.Modules
{
    public class DependencyAggregateModule : IDependencyModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddModule<ServicesModule>();
        }
    }
}
