using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bonfire.Analytics.Dto.Models;
using Sitecore;
using Sitecore.Analytics;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Campaigns;
using Sitecore.Marketing.Definitions.PageEvents;
using Sitecore.Marketing.Taxonomy;
using Sitecore.Marketing.Taxonomy.Extensions;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly IChannelTaxonomyManager channelTaxonomyManager;
        private readonly IDefinitionManager<ICampaignActivityDefinition> campaignDefinitionManager;

        public CampaignRepository()
        {
            this.channelTaxonomyManager = DependencyResolver.Current.GetService<IChannelTaxonomyManager>();
            this.campaignDefinitionManager = DependencyResolver.Current.GetService<IDefinitionManager<ICampaignActivityDefinition>>();
        }

        public Campaign GetCurrent()
        {
            if (!Tracker.Current.Interaction.CampaignId.HasValue)
            {
                return null;
            }
            var campaignId = Tracker.Current.Interaction.CampaignId.Value;
            var campaign = GetCampaignDefinition(campaignId);

            return new Campaign
            {
                Title = campaign?.Name ?? "(Unknown)",
                IsActive = true,
                Date = Tracker.Current.Interaction.StartDateTime,
                Channel = this.GetChannel(campaign)
            };
        }

        public IEnumerable<Campaign> GetHistoric()
        {
            var keyBehaviourCache = Tracker.Current.Contact.KeyBehaviorCache;
            foreach (var cachedCampaign in keyBehaviourCache.Campaigns)
            {
                var campaign = GetCampaignDefinition(cachedCampaign.Id);

                yield return new Campaign
                {
                    Title = campaign?.Name ?? "(Unknown)",
                    IsActive = false,
                    Date = cachedCampaign.DateTime,
                    Channel = this.GetChannel(campaign)
                };
            }
        }

        private string GetChannel(ICampaignActivityDefinition campaign)
        {
            if (campaign?.ChannelUri == null)
            {
                return null;
            }
            var channel = channelTaxonomyManager.GetChannel(campaign.ChannelUri, Context.Language.CultureInfo);
            return channel == null ? null : channelTaxonomyManager.GetFullName(channel.Uri, "/");
        }

        private ICampaignActivityDefinition GetCampaignDefinition(Guid campaignId)
        {
            var campaign = campaignDefinitionManager.Get(campaignId, Context.Language.CultureInfo) ?? campaignDefinitionManager.Get(campaignId, CultureInfo.InvariantCulture);
            return campaign;
        }
    }
}
