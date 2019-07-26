using Bonfire.Analytics.Dto.Models;
using Bonfire.Analytics.Dto.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Bonfire.Analytics.Dto.Controllers 
{
    using System;
    using System.Web.Mvc;
    using Bonfire.Analytics.Dto.Repositories;
    using Sitecore.Marketing.Definitions;

    public class BonfireDtoController : Controller
    {
        private readonly IContactRepository contactRepository;

        public BonfireDtoController()
        {
             this.contactRepository = new ContactRepository(DependencyResolver.Current.GetService<IServiceProvider>());
        }

        public ActionResult Details()
        {
            var model = new TrackerDto();
            try
            {
                var contactRepository = new ContactRepository(DependencyResolver.Current.GetService<IServiceProvider>());
                model = contactRepository.GetTrackerDto();
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
    }
}