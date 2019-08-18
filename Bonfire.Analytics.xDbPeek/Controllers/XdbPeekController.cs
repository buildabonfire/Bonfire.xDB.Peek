using System;
using System.Web.Mvc;
using Bonfire.Analytics.XdbPeek.Models;
using Bonfire.Analytics.XdbPeek.Repositories;

namespace Bonfire.Analytics.XdbPeek.Controllers 
{
    public class XdbPeekController : Controller
    {
        private readonly IContactRepository contactRepository;

        public XdbPeekController()
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