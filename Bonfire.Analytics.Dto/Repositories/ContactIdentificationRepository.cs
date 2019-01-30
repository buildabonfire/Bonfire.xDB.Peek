using System.Linq;
using Sitecore.Analytics;
using Sitecore.Analytics.Model;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.XConnect;
using Sitecore.XConnect.Client.Configuration;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class ContactIdentificationRepository : IContactIdentificationRepository
    {
        private readonly ContactManager contactManager;

        public ContactManager Manager => contactManager;

        public ContactIdentificationRepository()
        {
            contactManager = Factory.CreateObject("tracking/contactManager", true) as ContactManager;
        }

        public IdentifiedContactReference GetContactReference()
        {
            // get the contact id from the current contact
            var id = GetContactId();

            // if the contact is new or has no identifiers
            var anon = Tracker.Current.Contact.IsNew || Tracker.Current.Contact.Identifiers.Count == 0;

            // if the user is anon, get the xD.Tracker identifier, else get the one we found
            return anon
                ? new IdentifiedContactReference(Sitecore.Analytics.XConnect.DataAccess.Constants.IdentifierSource, Tracker.Current.Contact.ContactId.ToString("N"))
                : new IdentifiedContactReference(id.Source, id.Identifier);
        }

        public Sitecore.Analytics.Model.Entities.ContactIdentifier GetContactId()
        {
            if (Tracker.Current?.Contact == null)
            {
                return null;
            }
            if (Tracker.Current.Contact.IsNew)
            {
                // write the contact to xConnect so we can work with it
                this.SaveContact();
            }

            return Tracker.Current.Contact.Identifiers.FirstOrDefault();
        }

        public void SaveContact()
        {
            // we need the contract to be saved to xConnect. It is only in session now
            Tracker.Current.Contact.ContactSaveMode = ContactSaveMode.AlwaysSave;
            this.contactManager.SaveContactToCollectionDb(Tracker.Current.Contact);
        }

        public IXdbContext CreateContext()
        {
            return SitecoreXConnectClientConfiguration.GetClient();
        }

        public Sitecore.XConnect.Contact CreateContact(string email)
        {
            var reference = new ContactIdentifier("emailaddress", email, ContactIdentifierType.Known);
            var contact = new Sitecore.XConnect.Contact(reference);
            return contact;
        }
    }
}