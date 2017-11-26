

using System;
using System.Collections.Generic;
using Sitecore.Analytics.Core;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.Model.Entities;
using Sitecore.Analytics.Model.Framework;
using Sitecore.Analytics.Tracking;
using Sitecore.Data;

namespace Bonfire.Analytics.Dto.Models
{
    [Serializable]
    public class TrackerDto
    {
        public Contact Contact { get; set; }
        public CurrentPage CurrentPage { get; set; }
        public Interactions Interaction { get; set; }

        public bool IsActive { get; set; }

        public Session Session { get; set; }

        public string Campaign { get; set; }

        public List<GenericLink> PagesViewed { get; set; }

        public List<string> GoalsList { get; set; }

        public List<string> EngagementStates { get; set; }

    }

    public class Contact
    {
        public Guid ContactId { get; set; }
        public ContactSaveMode ContactSaveMode { get; set; }
        public IContactExtensionsContext Extensions { get; set; }
        public IReadOnlyDictionary<string, IFacet> Facets { get; set; }
        public IReadOnlyCollection<ContactIdentifier> Identifiers { get; set; }
        public bool IsTemporaryInstance { get; set; }
        public IContactSystemInfoContext System { get; set; }
        public IContactTagsContext Tags { get; set; }
        public List<ExtraBehaviorProfileContext> Profiles { get; set; }
        public List<ExtraBehaviorProfileContext> InteractionProfiles { get; set; }

    }

    public class BehaviorProfiles
    {
        public IEnumerable<IBehaviorProfileContext> Profiles { get; set; }
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

        public IVisitProfiles Profiles { get; set; }

        public ScreenInformationBase ScreenInfo { get; set; }

        public string SiteName { get; set; }

        public int Value { get; set; }
    }

    public class Session
    {
        public Interactions Interaction { get; set; }
        public Contact Contact { get; set; }
    }

    public class GenericLink
    {
        public GenericLink(string title, string url, bool openInBlankWindow)
        {
            Title = title;
            Url = url;
            OpenInBlankWindow = openInBlankWindow;
        }

        public string Title
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public bool OpenInBlankWindow
        {
            get;
            set;
        }
    }
}