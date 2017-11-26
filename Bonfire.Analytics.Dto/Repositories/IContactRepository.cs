using System.Collections.Generic;
using Bonfire.Analytics.Dto.Models;
using Sitecore.Analytics.Tracking;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.AutomationPlans.Model;
using Contact = Bonfire.Analytics.Dto.Models.Contact;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface IContactRepository
    {
        IDefinitionManager<IAutomationPlanDefinition> AutomationPlanDefinitionManager { get; }
        TrackerDto GetTrackerDto();
        IVisitProfiles GetTrackerDtoProfiles();
        Interactions GetInteractions(CurrentInteraction currentInteraction);
        Contact GetContact(Sitecore.Analytics.Tracking.Contact currectContact);
        string GetCampaign(CurrentInteraction currentInteraction);
        List<GenericLink> LoadPages();
        List<string> LoadGoals();
        List<string> LoadEngagementStates();
    }
}