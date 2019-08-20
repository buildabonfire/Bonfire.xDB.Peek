using Bonfire.Analytics.XdbPeek.Controllers;
using Bonfire.Analytics.XdbPeek.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Bonfire.Analytics.XdbPeek.ServiceConfigurator
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