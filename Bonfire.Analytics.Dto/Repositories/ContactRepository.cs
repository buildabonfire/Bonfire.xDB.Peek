using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bonfire.Analytics.Dto.Extensions;
using Bonfire.Analytics.Dto.Models;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.AutomationPlans.Model;
using Sitecore.XConnect.Collection.Model;
using Sitecore.Xdb.MarketingAutomation.Tracking.Extensions;
using Contact = Bonfire.Analytics.Dto.Models.Contact;
using Session = Bonfire.Analytics.Dto.Models.Session;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class ContactRepository : IContactRepository
    {
        public IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }

        public ContactRepository(IServiceProvider serviceProvider)
        {
            AutomationPlanDefinitionManager = serviceProvider.GetDefinitionManagerFactory().GetDefinitionManager<IAutomationPlanDefinition>();
        }

        public TrackerDto GetTrackerDto()
        {
            var currentTracker = Tracker.Current;

            var trackerDto = new TrackerDto
            {
                CurrentPage = new CurrentPage { Url = currentTracker.CurrentPage.Url },
                Interaction = GetInteractions(currentTracker.Interaction),
                IsActive = currentTracker.IsActive,
                Session = CreateSession(currentTracker),
                Campaign = GetCampaign(currentTracker.Interaction),
                Contact = GetContact(currentTracker.Contact),
                PagesViewed = LoadPages(),
                GoalsList = LoadGoals(),
                EngagementStates = LoadEngagementStates()
            };

            return trackerDto;
        }



        public IVisitProfiles GetTrackerDtoProfiles()
        {
            var currentTracker = Tracker.Current;
            return currentTracker.Interaction.Profiles;
        }

        public Interactions GetInteractions(CurrentInteraction currentInteraction)
        {
            var interactions = new Interactions
            {
                BrowserInfo = currentInteraction.BrowserInfo,
                CampaignId = currentInteraction.CampaignId,
                ContactId = currentInteraction.ContactId,
                ChannelId = currentInteraction.ChannelId,
                ContactVisitIndex = currentInteraction.ContactVisitIndex,
                CustomValues = currentInteraction.CustomValues,
                DeviceId = currentInteraction.DeviceId,
                GeoData = currentInteraction.GeoData,
                HasGeoIpData = currentInteraction.HasGeoIpData,
                InteractionId = currentInteraction.InteractionId,
                Ip = currentInteraction.Ip,
                Keywords = currentInteraction.Keywords,
                Language = currentInteraction.Language,
                Profiles = currentInteraction.Profiles,
                ScreenInfo = currentInteraction.ScreenInfo,
                SiteName = currentInteraction.SiteName,
                Value = currentInteraction.Value
            };


            return interactions;
        }

        public Contact GetContact(Sitecore.Analytics.Tracking.Contact currectContact)
        {
            var contact = new Contact
            {
                Profiles = currectContact.BehaviorProfiles.Profiles.Select(CreateExtraBehaviorProfileContext).ToList(),
                ContactId = currectContact.ContactId,
                ContactSaveMode = currectContact.ContactSaveMode,
                Extensions = currectContact.Extensions,
                Facets = currectContact.Facets,
                Identifiers = currectContact.Identifiers,
                IsTemporaryInstance = currectContact.IsTemporaryInstance,
                System = currectContact.System,
                Tags = currectContact.Tags
            };


            var items = Tracker.Current.Interaction.Profiles.GetProfileNames();

            var interactionProfiles = (from profileItem in items
                select Tracker.Current.Interaction.Profiles[profileItem]
                into profile
                let scores = profile.ToList()
                select new ExtraBehaviorProfileContext
                {
                    PatterneName = profile.PatternLabel,
                    ProfileName = profile.ProfileName,
                    Total = profile.Total,
                    PatternId = profile.PatternId.ToId(),
                    NumberOfTimesScored = profile.Count,
                    StringScore = scores
                }).ToList();

            contact.InteractionProfiles = interactionProfiles;
            return contact;
        }

        

        public string GetCampaign(CurrentInteraction currentInteraction)
        {
            if (currentInteraction.CampaignId.HasValue)
            {
                Item campaign = Context.Database.GetItem(currentInteraction.CampaignId.ToId());
                if (campaign != null) return campaign.Name;
            }

            return "Current Campaign Empty";
        }

        public List<GenericLink> LoadPages()
        {
            var pagesViewed = new List<GenericLink>();
            foreach (IPageContext page in Tracker.Current.Interaction.GetPages())
            {
                GenericLink link = new GenericLink(CleanPageName(page), page.Url.Path, false);
                pagesViewed.Add(link);
            }
            pagesViewed.Reverse();
            return pagesViewed;
        }

        public List<string> LoadGoals()
        {
            List<string> goals = new List<string>();

            var conversions = (from page in Tracker.Current.Interaction.GetPages()
                               from pageEventData in page.PageEvents
                               where pageEventData.IsGoal
                               select pageEventData).ToList();

            if (conversions.Any())
            {
                conversions.Reverse();
                foreach (var goal in conversions)
                {
                    goals.Add($"{goal.Name} ({goal.Value})");
                }
            }
            else
            {
                goals.Add("No Goals");
            }

            return goals;
        }

        public List<string> LoadEngagementStates()
        {
            var states = new List<string>();

            try
            {
                var plans = Tracker.Current?.Contact?.GetPlanEnrollmentCache();
                var enrollments = plans?.ActivityEnrollments;
                
                //var engagementstates = AutomationStateManager.Create(Tracker.Current.Contact).GetAutomationStates();

                if (enrollments != null && enrollments.Any())
                {
                    states = enrollments.Select(CreateEngagementPlanState).Select(x => x.Name).ToList();
                }
                else
                {
                    states.Add("No Engagement States");
                }
            }
            catch (Exception)
            {
                states.Add("Unable to load Engagement States");
            }
            return states;
        }

        private IAutomationPlanDefinition CreateEngagementPlanState(AutomationPlanActivityEnrollmentCacheEntry enrollment)
        {
            return AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId, Context.Language.CultureInfo) ?? AutomationPlanDefinitionManager.Get(enrollment.AutomationPlanDefinitionId, CultureInfo.InvariantCulture);
        }

        private string CleanPageName(IPageContext p)
        {
            string pageName = p.Url.Path.Replace("/en", "/").Replace("//", "/").Remove(0, 1).Replace(".aspx", "");
            if (pageName == String.Empty || pageName == "en") pageName = "Home";
            if (pageName.Contains("/"))
            {
                //pageName.Substring(0, pageName.IndexOf("/") + 1) +
                pageName = "..." + pageName.Substring(pageName.LastIndexOf("/", StringComparison.Ordinal));
            }
            return (pageName.Length < 27) ? $"{pageName} ({(p.Duration/1000.0).ToString("f2")}s)"
                :
                $"{pageName.Substring(0, 26)}... ({(p.Duration/1000.0).ToString("f2")}s)";
        }

        private static ExtraBehaviorProfileContext CreateExtraBehaviorProfileContext(IBehaviorProfileContext profile)
        {
            return new ExtraBehaviorProfileContext
            {
                Id = profile.Id,
                NumberOfTimesScored = profile.NumberOfTimesScored,
                Scores = profile.Scores,
                PatternId = profile.PatternId,
                ProfileName = Context.Database.GetItem(profile.Id).Name,
                PatterneName = (!ID.IsNullOrEmpty(profile.PatternId)) ? Context.Database.GetItem(profile.PatternId).Name : ""
            };
        }

        private Session CreateSession(ITracker currentTracker)
        {
            return new Session
            {
                Contact = GetContact(currentTracker.Session.Contact),
                Interaction = GetInteractions(currentTracker.Session.Interaction)
            };
        }
    }
}
