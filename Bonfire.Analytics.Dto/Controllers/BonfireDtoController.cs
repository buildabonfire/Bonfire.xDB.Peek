using System.Web.Mvc;
using Bonfire.Analytics.Dto.Repositories;

namespace Bonfire.Analytics.Dto.Controllers 
{
    public class BonfireDtoController : Controller
    {
        private readonly IContactRepository contactRepository;

        public BonfireDtoController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        public ActionResult Details()
        {
            var model = contactRepository.GetTrackerDto();

            return View(model);
        }
    }
}