using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bonfire.Analytics.Dto.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Schema;

namespace Bonfire.Analytics.Dto.Repositories
{
    public class FacetRepository : IFacetRepository
    {
        private readonly ContactIdentificationRepository _contactIdentificationRepository;

        public FacetRepository()
        {
            _contactIdentificationRepository = new ContactIdentificationRepository();
        }

        public Dictionary<string, object> GetFacets()
        {

            return null;
        }

        //public IReadOnlyDictionary<string, Facet> GetClientFacets()
        //{
        //    var contactReference = _contactIdentificationRepository.GetContactReference();

        //    var facetModels = GetAllContactFacetModels();

        //    // get all the facets from the configuration api client
        //    var facets = facetModels.Select(x => x.Name).ToList();

        //    using (var client = _contactIdentificationRepository.CreateContext())
        //    {
        //        var contact = client.Get(contactReference, new ContactExpandOptions(facets.ToArray()));

        //        foreach (var contactFacet in contact.Facets)
        //        {
        //            var props = contactFacet.Value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Static);

        //            foreach (var propertyInfo in props)
        //            {
        //                var name = propertyInfo.Name;
        //                var val = propertyInfo.GetValue(contactFacet, null);
        //            }
        //        }

        //        return null;
        //    }
        //}

        //public static string[] PropertiesFromType(object atype)
        //{
        //    if (atype == null) return new string[] { };
        //    Type t = atype.GetType();
        //    PropertyInfo[] props = t.GetProperties();
        //    List<string> propNames = new List<string>();
        //    foreach (PropertyInfo prp in props)
        //    {
        //        propNames.Add(prp.Name);
        //    }
        //    return propNames.ToArray();
        //}

        public IEnumerable<XdbFacetDefinition> GetAllContactFacetModels()
        {
            return this.GetAllFacetModels().Where(x => x.Target == EntityType.Contact);
        }

        public IEnumerable<XdbFacetDefinition> GetAllInteractionFacetModels()
        {
            return this.GetAllFacetModels().Where(x => x.Target == EntityType.Interaction);
        }

        public List<XdbFacetDefinition> GetAllFacetModels()
        {
            var models = new List<XdbFacetDefinition>();
            var facets = GetFacetModels();

            foreach (var xdbModel in facets)
            {
                models.AddRange(xdbModel.Facets);
            }

            return models;
        }

        public IEnumerable<XdbModel> GetFacetModels()
        {
            using (var client = _contactIdentificationRepository.CreateContext())
            {
                var configuration = (XConnectClientConfiguration)client.Configuration;

                return configuration.ConfigurationClient.KnownModels;
            }
        }

        public List<FacetModel> GetFacets(List<XdbFacetDefinition> facetDefinitions)
        {
            return facetDefinitions.Select(CreateFacetModel).ToList();
        }

        private FacetModel CreateFacetModel(XdbFacetDefinition facetDefinition)
        {
            return new FacetModel()
            {
                LocalName = facetDefinition.FacetType.LocalName
            };
        }

        private FacetModelModel CreateFacetModel(XdbModel model)
        {
            return new FacetModelModel()
            {
                FacetModelName = model.FullName.Name,
                Version = model.FullName.Version,
                FacetName = model.Name
            };
        }

    }
}