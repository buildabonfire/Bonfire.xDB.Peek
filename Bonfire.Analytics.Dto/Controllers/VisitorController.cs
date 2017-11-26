namespace Bonfire.Analytics.Dto.Controllers
{
    using System.Web.Mvc;
    using Repositories;

    public class VisitorController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public VisitorController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public JsonResult VisitorDetailsJson()
        {
            var trackerDto = _contactRepository.GetTrackerDto();

            return Json(trackerDto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClearVisitorSession()
        {
            Session.Abandon();

            return Json("Done", JsonRequestBehavior.AllowGet);
        }
    }
}
