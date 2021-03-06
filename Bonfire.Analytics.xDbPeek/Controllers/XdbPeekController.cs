﻿using System;
using System.Web.Mvc;
using Bonfire.Analytics.XdbPeek.Models;
using Bonfire.Analytics.XdbPeek.Repositories;

namespace Bonfire.Analytics.XdbPeek.Controllers 
{
    public class XdbPeekController : Controller
    {
        public XdbPeekController()
        {
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
            catch
            {
                return View(model);
            }
        }
    }
}