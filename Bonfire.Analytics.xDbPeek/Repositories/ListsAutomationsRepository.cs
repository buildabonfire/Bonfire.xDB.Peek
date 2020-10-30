using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Bonfire.Analytics.XdbPeek.Models;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.AutomationPlans.Model;
using Sitecore.XConnect.Collection.Model;
using Sitecore.Xdb.MarketingAutomation.Tracking.Extensions;

namespace Bonfire.Analytics.XdbPeek.Repositories
{
    public class ListsAutomationsRepository : IListsAutomationsRepository
    {
        private IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }

        public ListsAutomationsRepository()
        {
            AutomationPlanDefinitionManager =
                DependencyResolver.Current.GetService<IDefinitionManager<IAutomationPlanDefinition>>();
        }

        public IEnumerable<ListsAutomations> GetCurrent()
        {
            var plans = Tracker.Current?.Contact?.GetPlanEnrollmentCache();
            var enrollments = plans?.ActivityEnrollments;

            return enrollments?.Select(CreateEngagementPlanState).ToArray() ?? Enumerable.Empty<ListsAutomations>();
        }

        private ListsAutomations CreateEngagementPlanState(AutomationPlanActivityEnrollmentCacheEntry enrollment)
        {
            var definition =
                AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId,
                    Context.Language.CultureInfo) ??
                AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId,
                    CultureInfo.InvariantCulture);
            //var activity = definition?.GetActivity(enrollment.ActivityId);
            //var activityName = activity?.Parameters?["Name"] != null ? activity.Parameters["Name"]?.ToString() : string.Empty;

            var state = new ListsAutomations();
            state.EngagementPlanTitle = definition?.Name;
            state.Title = "";
            state.Date = enrollment.ActivityEntryDate;
            return state;
        }
    }
}