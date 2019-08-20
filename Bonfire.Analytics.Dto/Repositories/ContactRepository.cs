using System;
using System.Collections.Generic;
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

namespace Bonfire.Analytics.Dto.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly IContactIdentificationRepository contactIdentificationRepository;
        private readonly IFacetRepository facetRepository;
        private readonly IEventRepository eventRepository;
        private readonly IEngagementPlanStateRepository engagementPlanStateRepository;
        private readonly ICampaignRepository campaignRepository;

        public IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }

        public ContactRepository(IServiceProvider serviceProvider)
        {
            AutomationPlanDefinitionManager = serviceProvider.GetDefinitionManagerFactory().GetDefinitionManager<IAutomationPlanDefinition>();
            contactIdentificationRepository = new ContactIdentificationRepository();
            facetRepository = new FacetRepository();
            eventRepository = new EventRepository();
            engagementPlanStateRepository = new EngagementPlanStateRepository();
            campaignRepository = new CampaignRepository();
        }

        public TrackerDto GetTrackerDto()
        {
            var currentTracker = Tracker.Current;

            var trackerDto = new TrackerDto
            {
                CurrentPage = new CurrentPage { Url = currentTracker.CurrentPage.Url },
                IsActive = currentTracker.IsActive,
                Contact = GetContact(),
                Interaction = GetInteractions(currentTracker.Interaction),
                Facets = this.GetContact().Facets.ToList(),
                PagesViewed = LoadPages(),
                GoalsList = eventRepository.GetCurrentGoals().ToList(),
                PastGoals = eventRepository.GetHistoricGoals().ToList(),
                EngagementPlanStates = engagementPlanStateRepository.GetCurrent(),
                CurrentCampaign = campaignRepository.GetCurrent(),
                PastCampaigns = campaignRepository.GetHistoric(),
                CurrentProfiles = GetCurrentProfiles(),
                PastProfiles = GetPastProfiles()
            };

            return trackerDto;
        }

        public Interactions GetInteractions(CurrentInteraction currentInteraction)
        {
            var interactions = new Interactions
            {
                BrowserInfo = currentInteraction.BrowserInfo,
                CampaignId = currentInteraction.CampaignId,
                ContactId = currentInteraction.ContactId,
                ChannelName = Context.Database.GetItem(ID.Parse(currentInteraction.ChannelId)).Name,
                ContactVisitIndex = currentInteraction.ContactVisitIndex,
                CustomValues = currentInteraction.CustomValues,
                DeviceId = currentInteraction.DeviceId,
                GeoData = currentInteraction.GeoData,
                HasGeoIpData = currentInteraction.HasGeoIpData,
                InteractionId = currentInteraction.InteractionId,
                Ip = currentInteraction.Ip,
                Keywords = currentInteraction.Keywords,
                Language = currentInteraction.Language,
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
                PatternName = (!ID.IsNullOrEmpty(profile.PatternId)) ? Context.Database.GetItem(profile.PatternId).Name : ""
            };
        }

        private static List<PatternProfile> GetCurrentProfiles()
        {
            var profileNames = Tracker.Current.Interaction.Profiles.GetProfileNames();
            var profile = profileNames.Select(p => Tracker.Current.Interaction.Profiles[p]);
            return profile.Select(CreatePatternProfile).ToList();
        }

        private static IEnumerable<ExtraBehaviorProfileContext> GetPastProfiles()
        {
            return Tracker.Current.Contact.BehaviorProfiles.Profiles.Select(CreateExtraBehaviorProfileContext);
        }

        private static PatternProfile CreatePatternProfile(Profile profile)
        {
            return new PatternProfile
            {
                Count = profile.Count,
                PatternName = profile.PatternId != null ? Context.Database.GetItem(profile.PatternId.ToId()).Name : "",
                ProfileName = profile.ProfileName,
                Score = profile.Total,
                PatternId = profile.PatternId,
                PatternLabel = profile.PatternLabel
            };
        }
    }
}
