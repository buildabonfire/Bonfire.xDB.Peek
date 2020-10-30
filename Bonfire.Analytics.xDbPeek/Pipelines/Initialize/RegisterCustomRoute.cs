using System.Web.Mvc;
using System.Web.Routing;
using Sitecore.Mvc.Pipelines.Loader;
using Sitecore.Pipelines;

namespace Bonfire.Analytics.XdbPeek.Pipelines.Initialize
{
    public class RegisterCustomRoute : InitializeRoutes
    {
        private const string BaseRoute = "apis";
        private const string ApiVersion = "/v1";

        public override void Process(PipelineArgs args)
        {
            RouteTable.Routes.MapRoute(
                "bonfire-VisitorData",
                BaseRoute + ApiVersion + "/VisitorDetails",
                new { controller = "Visitor", action = "VisitorDetailsJson" });

            RouteTable.Routes.MapRoute(
                "bonfire-ClearVisitorSession",
                BaseRoute + ApiVersion + "/ClearVisitorSession",
                new { controller = "Visitor", action = "ClearVisitorSession" });

            RouteTable.Routes.MapRoute(
                "bonfire-MakeHuman",
                BaseRoute + ApiVersion + "/MakeSessionHuman",
                new { controller = "Visitor", action = "SetVisitHuman" });

            RouteTable.Routes.MapRoute(
                "bonfire-MakeRobot",
                BaseRoute + ApiVersion + "/MakeSessionRobot",
                new { controller = "Visitor", action = "SetVisitRobot" });

            RouteTable.Routes.MapRoute(
                "bonfire-GetListName",
                BaseRoute + ApiVersion + "/GetListName",
                new { controller = "Visitor", action = "GetListName" });

            RouteTable.Routes.MapRoute(
                "bonfire-VisitorDetails",
                "VisitorDetails",
                new { controller = "XdbPeek", action = "Details" });
        }
    }
}
