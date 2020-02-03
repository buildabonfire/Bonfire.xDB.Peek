using System;
using System.Web.Mvc;
using Bonfire.Analytics.XdbPeek.Repositories;
using Bonfire.Analytics.XdbPeek.Serialization;
using Sitecore.Analytics;

namespace Bonfire.Analytics.XdbPeek.Controllers
{
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

        [HttpGet]
        public JsonResult SetVisitHuman()
        {
            Tracker.Current.Session.SetClassification(0, 0, true);

            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetListName(Guid id)
        {
            var response = contactRepository.GetListName(id);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
