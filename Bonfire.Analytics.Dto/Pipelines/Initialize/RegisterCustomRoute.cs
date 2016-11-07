using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Pipelines;

namespace Bonfire.Analytics.Dto.Pipelines.Initialize
{
    public class RegisterCustomRoute : Sitecore.Mvc.Pipelines.Loader.InitializeRoutes
    {
        private const string BASE_ROUTE = "apis";
        private const string API_VERSION = "/v1";

        public override void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute(
                "VisitorData",
                BASE_ROUTE + API_VERSION + "/VisitorDetails",
                new { controller = "Test", action = "VisitorDetailsJson" },
                new[] { "Bonfire.Analytics.Dto.Controllers" });

            RouteTable.Routes.MapRoute(
                "ClearVisitorSession",
                BASE_ROUTE + API_VERSION + "/ClearVisitorSession",
                new { controller = "Test", action = "ClearVisitorSession" },
                new[] { "Bonfire.Analytics.Dto.Controllers" });
        }
    }
}
