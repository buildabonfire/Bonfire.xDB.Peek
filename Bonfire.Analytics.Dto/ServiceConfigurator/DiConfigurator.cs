using Bonfire.Analytics.Dto.Controllers;
using Bonfire.Analytics.Dto.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Bonfire.Analytics.Dto.ServiceConfigurator
{
    public class DiConfigurator : IServicesConfigurator
    {

        public void Configure(IServiceCollection serviceCollection)
        {
            // register each repository and service
            serviceCollection.AddScoped<IContactRepository, ContactRepository>();
            serviceCollection.AddTransient(typeof(VisitorController));
        }
    }
}