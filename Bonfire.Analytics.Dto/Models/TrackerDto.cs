namespace Bonfire.Analytics.Dto.Models
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Analytics.Core;
    using Sitecore.Analytics.Model;
    using Sitecore.Analytics.Tracking;
    using Sitecore.Data;

    [Serializable]
    public class TrackerDto
    {
        public Sitecore.XConnect.Contact Contact { get; set; }
        public CurrentPage CurrentPage { get; set; }
        public Interactions Interaction { get; set; }
        public bool IsActive { get; set; }
        public Session Session { get; set; }
        public IEnumerable<Campaign> PastCampaigns { get; set; }
        public Campaign CurrentCampaign { get; set; }
        public List<GenericLink> PagesViewed { get; set; }
        public List<PageEvent> GoalsList { get; set; }
        public List<PageEvent> PastGoals { get; set; }
        public List<KeyValuePair<string, Sitecore.XConnect.Facet>> Facets { get; set; }
        public List<PatternProfile> CurrentProfiles { get; set; }
        public IEnumerable<ExtraBehaviorProfileContext> PastProfiles { get; set; }
        public IEnumerable<EngagementPlanState> EngagementPlanStates { get; set; }
    }

    public class ExtraBehaviorProfileContext
    {
        public string ProfileName { get; set; }
        public ID Id { get; set; }
        public int NumberOfTimesScored { get; set; }
        public double Total { get; set; }
        public IEnumerable<KeyValuePair<ID, double>> Scores { get; set; }
        public IEnumerable<KeyValuePair<string, double>> StringScore { get; set; }
        public string PatterneName { get; set; }
        public ID PatternId { get; set; }
    }

    public class CurrentPage
    {
        public UrlData Url { get; set; }
    }

    public class Interactions
    {
        public BrowserInformationBase BrowserInfo { get; set; }

        public Guid? CampaignId { get; set; }

        public Guid ContactId { get; set; }

        public Guid ChannelId { get; set; }

        public int ContactVisitIndex { get; set; }

        public IDictionary<string, object> CustomValues { get; set; }

        public Guid DeviceId { get; set; }

        public ContactLocation GeoData { get; set; }

        public bool HasGeoIpData { get; set; }

        public Guid InteractionId { get; set; }

        public byte[] Ip { get; set; }

        public string Keywords { get; set; }

        public string Language { get; set; }

        public ScreenInformationBase ScreenInfo { get; set; }

        public string SiteName { get; set; }

        public int Value { get; set; }
    }

    public class GenericLink
    {
        public GenericLink(string title, string url, bool openInBlankWindow)
        {
            Title = title;
            Url = url;
            OpenInBlankWindow = openInBlankWindow;
        }

        public string Title { get; set; }

        public string Url { get; set; }

        public bool OpenInBlankWindow { get; set; }
    }
}