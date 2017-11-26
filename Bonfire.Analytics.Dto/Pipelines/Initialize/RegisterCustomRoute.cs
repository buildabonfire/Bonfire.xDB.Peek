using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Mvc.Pipelines.Loader;
using Sitecore.Pipelines;

namespace Bonfire.Analytics.Dto.Pipelines.Initialize
{
    public class RegisterCustomRoute : InitializeRoutes
    {
        private const string BaseRoute = "apis";
        private const string ApiVersion = "/v1";

        public override void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute(
                "VisitorData",
                BaseRoute + ApiVersion + "/VisitorDetails",
                new { controller = "Visitor", action = "VisitorDetailsJson" },
                new[] { "Bonfire.Analytics.Dto.Controllers" });

            RouteTable.Routes.MapRoute(
                "ClearVisitorSession",
                BaseRoute + ApiVersion + "/ClearVisitorSession",
                new { controller = "Visitor", action = "ClearVisitorSession" },
                new[] { "Bonfire.Analytics.Dto.Controllers" });
        }
    }
}
