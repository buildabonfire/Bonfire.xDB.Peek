using System;
using System.Collections.Generic;
using System.Reflection;
using Bonfire.Analytics.Dto.Extensions;
using Bonfire.Analytics.Dto.Models;
using Bonfire.Analytics.Dto.Serialization;
using DocumentFormat.OpenXml.ExtendedProperties;
using Newtonsoft.Json;
using Sitecore.Analytics.Data.Items;
using Sitecore.XConnect;

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
