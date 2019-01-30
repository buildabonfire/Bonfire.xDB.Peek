using Sitecore.Analytics.Tracking;
using Sitecore.XConnect;

namespace Bonfire.Analytics.Dto.Repositories
{
    public interface IContactIdentificationRepository
    {
        ContactManager Manager { get; }
        IdentifiedContactReference GetContactReference();
        Sitecore.Analytics.Model.Entities.ContactIdentifier GetContactId();
        void SaveContact();
        IXdbContext CreateContext();
        Sitecore.XConnect.Contact CreateContact(string email);
    }
}