using Bonfire.Analytics.Dto.Serialization;

namespace Bonfire.Analytics.Dto.Controllers
{
    using System.Web.Mvc;
    using Repositories;

    public class VisitorController : Controller
    {
        private readonly IContactRepository contactRepository;

        public VisitorController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        [HttpGet]
        public ActionResult VisitorDetailsJson()
        {
            var trackerDto = contactRepository.GetTrackerDto();

            return new JsonNet(trackerDto);
        }

        [HttpGet]
        public JsonResult ClearVisitorSession()
        {
            Session.Abandon();

            return Json("Done", JsonRequestBehavior.AllowGet);
        }
    }
}
