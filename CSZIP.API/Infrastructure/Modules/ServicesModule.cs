using CSZIP.API.Domain.Interfaces;
using CSZIP.API.Domain.Interfaces.Managers;
using CSZIP.API.Domain.Interfaces.Services;
using CSZIP.API.Domain.Managers;
using CSZIP.API.Domain.Services;
using CSZIP.API.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSZIP.API.Infrastructure.Modules
{
    public class ServicesModule : IDependencyModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProcessingService, ProcessingService>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
        }
    }
}
