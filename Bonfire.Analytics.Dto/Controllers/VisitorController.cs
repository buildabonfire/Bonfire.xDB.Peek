using System;
using System.Collections.Generic;
using System.Reflection;
using Bonfire.Analytics.Dto.Extensions;
using Bonfire.Analytics.Dto.Models;
using Bonfire.Analytics.Dto.Serialization;
using DocumentFormat.OpenXml.ExtendedProperties;
using Newtonsoft.Json;
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

            //var facets = new List<KeyValuePair<string, List<KeyValuePair<string, object>>>>();

            //foreach (var contactFacet in trackerDto.Contact.Facets)
            //{
            //    var facetValues = new List<KeyValuePair<string, object>>();
            //    var allprops = contactFacet.Value.GetType().GetProperties();

            //    foreach (var propertyInfo in allprops)
            //    {
            //        if (propertyInfo.Name != "XObject")
            //            facetValues.Add(new KeyValuePair<string, object>(propertyInfo.Name, propertyInfo.GetValue(contactFacet.Value)));
            //    }

            //    facets.Add(new KeyValuePair<string, List<KeyValuePair<string, object>>>(contactFacet.Key, facetValues));
            //}




            //var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
            //jsonResolver.IgnoreProperty(typeof(XdbExtensible), "XObject");
            ////jsonResolver.RenameProperty(typeof(Person), "FirstName", "firstName");

            //var serializerSettings = new JsonSerializerSettings();
            //serializerSettings.ContractResolver = jsonResolver;

            //var json = JsonConvert.SerializeObject(trackerDto.Contact, serializerSettings);


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
