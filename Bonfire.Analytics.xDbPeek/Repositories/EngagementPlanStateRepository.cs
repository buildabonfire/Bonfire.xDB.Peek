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
    public class EngagementPlanStateRepository : IEngagementPlanStateRepository
    {
        private IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }

        public EngagementPlanStateRepository()
        {
            this.AutomationPlanDefinitionManager = DependencyResolver.Current.GetService<IDefinitionManager<IAutomationPlanDefinition>>();
        }

        public IEnumerable<EngagementPlanState> GetCurrent()
        {
            var plans = Tracker.Current?.Contact?.GetPlanEnrollmentCache();
            var enrollments = plans?.ActivityEnrollments;

            return enrollments?.Select(this.CreateEngagementPlanState).ToArray() ?? Enumerable.Empty<EngagementPlanState>();
        }

        private EngagementPlanState CreateEngagementPlanState(AutomationPlanActivityEnrollmentCacheEntry enrollment)
        {
            var definition = this.AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId, Context.Language.CultureInfo) ?? this.AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId, CultureInfo.InvariantCulture);
            var activity = definition?.GetActivity(enrollment.ActivityId);

            return new EngagementPlanState
            {
                EngagementPlanTitle = definition?.Name,
                Title = activity?.Parameters["Name"]?.ToString() ?? string.Empty,
                Date = enrollment.ActivityEntryDate
            };
        }
    }
}
