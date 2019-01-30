using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bonfire.Analytics.Dto.Extensions;
using Bonfire.Analytics.Dto.Models;
using Newtonsoft.Json;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.AutomationPlans.Model;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Serialization;
using Sitecore.XConnect.Collection.Model;
using Contact = Bonfire.Analytics.Dto.Models.Contact;
using Session = Bonfire.Analytics.Dto.Models.Session;
using System.Web.Script.Serialization;
using Sitecore.Extensions;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IContactIdentificationRepository contactIdentificationRepository;
        private readonly IFacetRepository facetRepository;
        private readonly IEventRepository eventRepository;
        public IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }

        public ContactRepository(IServiceProvider serviceProvider)
        {
            AutomationPlanDefinitionManager = serviceProvider.GetDefinitionManagerFactory().GetDefinitionManager<IAutomationPlanDefinition>();
            contactIdentificationRepository = new ContactIdentificationRepository();
            facetRepository = new FacetRepository();
            eventRepository = new EventRepository();
        }

        public TrackerDto GetTrackerDto()
        {
            var currentTracker = Tracker.Current;

            var trackerDto = new TrackerDto
            {
                CurrentPage = new CurrentPage { Url = currentTracker.CurrentPage.Url },
                //Interaction = GetInteractions(currentTracker.Interaction),
                IsActive = currentTracker.IsActive,
                //Session = CreateSession(currentTracker),
                //Campaign = GetCampaign(currentTracker.Interaction),
                Contact = GetContact(),
                Facets = this.GetContact().Facets.ToList(),
                PagesViewed = LoadPages(),
                GoalsList = eventRepository.GetCurrentGoals().ToList(),
                PastGoals = eventRepository.GetHistoricGoals().ToList()
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

        public Sitecore.XConnect.Contact GetContact()
        {
            var contactFacets = facetRepository.GetAllContactFacetModels();
            var facetList = contactFacets.Select(x => x.Name).Where(x => x != "KeyBehaviorCache").Distinct();

            var contactReference = this.contactIdentificationRepository.GetContactReference();

            
                using (var client = this.contactIdentificationRepository.CreateContext())
                {
                    var contact = client.Get(contactReference, new ContactExpandOptions(facetList.ToArray()));

                    var thing = JsonConvert.SerializeObject(contact);

                    var serializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new XdbJsonContractResolver(client.Model,
                            serializeFacets: true,
                            serializeContactInteractions: true),
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                        DefaultValueHandling = DefaultValueHandling.Ignore,
                        Formatting = Formatting.None
                    };

                    return contact;
            }
          
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
    }
}
