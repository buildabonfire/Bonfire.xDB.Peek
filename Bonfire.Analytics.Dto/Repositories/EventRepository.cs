using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Bonfire.Analytics.Dto.Models;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.PageEvents;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IDefinitionManager<IPageEventDefinition> pageEventDefinitionManager;

        public EventRepository()
        {
            pageEventDefinitionManager = DependencyResolver.Current.GetService<IDefinitionManager<IPageEventDefinition>>();
        }

        public IEnumerable<PageEvent> GetHistoricGoals()
        {
            var keyBehaviourCache = Tracker.Current.Contact.KeyBehaviorCache;
            foreach (var cachedGoal in keyBehaviourCache.Goals)
            {
                var goal = GetGoalDefinition(cachedGoal.Id);

                yield return new PageEvent
                {
                    Title = goal?.Name ?? "(Unknown)",
                    Date = cachedGoal.DateTime,
                    EngagementValue = goal?.EngagementValuePoints ?? 0,
                    IsCurrentVisit = false,
                    Data = cachedGoal.Data
                };
            }
        }

        public IEnumerable<PageEvent> GetCurrentGoals()
        {
            var conversions = (from page in Tracker.Current.Interaction.GetPages()
                from pageEventData in page.PageEvents
                where pageEventData.IsGoal
                select pageEventData).ToList();

            foreach (var cachedGoal in conversions)
            {
                var goal = GetGoalDefinition(cachedGoal.ItemId);

                yield return new PageEvent
                {
                    Title = cachedGoal.Name ?? "(Unknown)",
                    Date = cachedGoal.DateTime,
                    EngagementValue = goal?.EngagementValuePoints ?? 0,
                    IsCurrentVisit = false,
                    Data = cachedGoal.Data
                };
            }
        }

        public IPageEventDefinition GetGoalDefinition(Guid goalId)
        {
            return pageEventDefinitionManager.Get(goalId, Context.Language.CultureInfo) ?? pageEventDefinitionManager.Get(goalId, CultureInfo.InvariantCulture);
        }
    }
}