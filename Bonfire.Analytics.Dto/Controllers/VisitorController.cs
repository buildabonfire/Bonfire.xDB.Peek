using System.Web.Mvc;
using Bonfire.Analytics.Dto.Dto;
using Sitecore.Mvc.Controllers;

namespace Bonfire.Analytics.Dto.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public JsonResult VisitorDetailsJson()
        {
            var vi = new VisitorInformation();
            var trackerDto = vi.GetTrackerDto();

            return Json(trackerDto, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ClearVisitorSession()
        {
            Session.Abandon();

            return Json("Done");
        }
    }
}
