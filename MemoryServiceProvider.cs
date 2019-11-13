﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace BnBTechnologies.Xrm.MemoryServiceProvider
{
    class MemoryExecutionContext : IExecutionContext
    {
        private ParameterCollection inputparameters = null;
        private EntityImageCollection preentityimages = null;
        private EntityImageCollection postentityimages = null;

        public Guid UserId => Guid.NewGuid();

        public Guid CorrelationId => Guid.NewGuid();

        public int Depth => 1;

        public Guid InitiatingUserId => Guid.NewGuid();

        public ParameterCollection InputParameters
        {
            get
            {
                if (inputparameters == null)
                {
                    new ParameterCollection
                    {
                        new KeyValuePair<string, object>(
                            "Target",
                            new Microsoft.Xrm.Sdk.Entity()
                        ),
                        new KeyValuePair<string, object>(
                            "ConcurrencyBehavior",
                            Microsoft.Xrm.Sdk.ConcurrencyBehavior.Default
                        )
                    };
                }

                return inputparameters;
            }
            set
            {
                inputparameters = value;
            }
        }

        public bool IsExecutingOffline => false;

        public bool IsInTransaction => false;

        public bool IsOfflinePlayback => false;

        public int IsolationMode => 2;

        public string MessageName => "Create";

        public int Mode => 0;

        public DateTime OperationCreatedOn => DateTime.Now;

        public Guid OperationId => Guid.NewGuid();

        public Guid OrganizationId => Guid.NewGuid();

        public string OrganizationName => "Mock Organiation";

        public ParameterCollection OutputParameters => throw new NotImplementedException();

        public EntityReference OwningExtension => new EntityReference();

        public EntityImageCollection PreEntityImages
        {
            get
            {
                if (preentityimages == null)
                {
                    preentityimages = new EntityImageCollection
                    {
                        new KeyValuePair<string, Entity> (
                            "Target",
                            new Microsoft.Xrm.Sdk.Entity()
                        )
                    };
                }
                return preentityimages;
            }
            set
            {
                preentityimages = value;
            }
        }


        public EntityImageCollection PostEntityImages
        {
            get
            {
                if (postentityimages == null)
                {
                    postentityimages = new EntityImageCollection
                    {
                        new KeyValuePair<string, Entity>(
                            "Target",
                            new Microsoft.Xrm.Sdk.Entity()
                        )
                    };
                }
                return postentityimages;
            }
            set
            {
                postentityimages = value;
            }
        }

        public Guid PrimaryEntityId => Guid.NewGuid();

        public string PrimaryEntityName => "bnb_mockentity";

        public Guid? RequestId => Guid.NewGuid();

        public string SecondaryEntityName => "none";

        public ParameterCollection SharedVariables => new ParameterCollection();

        public Guid BusinessUnitId => Guid.NewGuid();

        public int Stage => 20;

        public string TraceInfo => null;
    }

    class ServiceEndpointNotificationService : IServiceEndpointNotificationService
    {
        public string Execute(EntityReference serviceEndpoint, IExecutionContext context)
        {
            return "Not applicable for Memory Service Provider ";
        }
    }

    class OrganizationService : IOrganizationService
    {
        public EntityCollection postEntityColl
        {
            get
            {
                if (_postentityCollection == null)
                    _postentityCollection = new EntityCollection();

                return _postentityCollection;
            }
            set
            {
                _postentityCollection = value;
            }
        }

        private EntityCollection _postentityCollection;
        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
        }

        public Guid Create(Entity entity)
        {
            entity.Id = Guid.NewGuid();
            postEntityColl.Entities.Add(entity);
            return entity.Id;
        }

        public void Delete(string entityName, Guid id)
        {
        }

        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
        {
        }

        public OrganizationResponse Execute(OrganizationRequest request)
        {
            OrganizationResponse resp = new OrganizationResponse();

            resp.ExtensionData = null;
            resp.ResponseName = "Mock Response";
            resp.Results = new ParameterCollection();

            return resp;

        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            Entity entity = new Entity(entityName);

            foreach (string str in columnSet.Columns)
            {
                entity.Attributes.Add(str, "0"); // o ise valid for integer, boolean and string
            }

            return entity;
        }

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            EntityCollection returnCollection = new EntityCollection();

            //supposing query returns 3 records for the entity
            for (int i = 1; i <= 3; i++)
            {
                Entity entity = new Entity(((QueryExpression)query).EntityName);
                foreach (string str in ((QueryExpression)query).ColumnSet.Columns)
                {
                    if (str.EndsWith("id"))
                    {
                        EntityReference entityRef = new EntityReference(str.TrimEnd("id".ToCharArray()).TrimEnd("ID".ToCharArray()).TrimEnd("Id".ToCharArray()));
                        entityRef.Id = Guid.NewGuid();
                        entity.Attributes.Add(str, entityRef);
                    }
                    else
                        entity.Attributes.Add(str, "0"); // 0 ise valid for integer, boolean and string
                }
                returnCollection.Entities.Add(entity);
            }

            return returnCollection;
        }

        public void Update(Entity entity)
        {
            postEntityColl.Entities.Add(entity);
        }
    }

    class OrganizationServiceFactory : IOrganizationServiceFactory
    {
        private OrganizationService orgserv = null;
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            if (orgserv == null)
                orgserv = new OrganizationService();
            return orgserv;
        }
    }

    class MemoryPluginExecutionContext : IPluginExecutionContext
    {
        private ParameterCollection inputparameters = null;
        private EntityImageCollection preentityimages = null;
        private EntityImageCollection postentityimages = null;

        public Guid UserId => Guid.NewGuid();

        public Guid CorrelationId => Guid.NewGuid();

        public int Depth => 1;

        public Guid InitiatingUserId => Guid.NewGuid();

        public ParameterCollection InputParameters
        {
            get
            {
                if (inputparameters == null)
                {
                    inputparameters = new ParameterCollection
                    {
                        new KeyValuePair<string, object>(
                            "Target",
                            new Microsoft.Xrm.Sdk.Entity()
                         ),
                        new KeyValuePair<string, object>(
                            "ConcurrencyBehavior",
                            Microsoft.Xrm.Sdk.ConcurrencyBehavior.Default
                        )
                    };
                }

                return inputparameters;
            }
            set
            {
                inputparameters = value;
            }
        }

        public bool IsExecutingOffline => false;

        public bool IsInTransaction => false;

        public bool IsOfflinePlayback => false;

        public int IsolationMode => 2;

        public string MessageName => "Create";

        public int Mode => 0;

        public DateTime OperationCreatedOn => DateTime.Now;

        public Guid OperationId => Guid.NewGuid();

        public Guid OrganizationId => Guid.NewGuid();

        public string OrganizationName => "Mock Organiation";

        public ParameterCollection OutputParameters => throw new NotImplementedException();

        public EntityReference OwningExtension => new EntityReference();

        public EntityImageCollection PreEntityImages
        {
            get
            {
                if (preentityimages == null)
                {
                    preentityimages = new EntityImageCollection
                    {
                        new KeyValuePair<string, Entity> (
                                            "Target",
                                            new Microsoft.Xrm.Sdk.Entity()
                                        )
                    };
                }
                return preentityimages;
            }
            set
            {
                preentityimages = value;
            }
        }

        public EntityImageCollection PostEntityImages
        {
            get
            {
                if (postentityimages == null)
                {
                    postentityimages = new EntityImageCollection
                    {
                        new KeyValuePair<string, Entity>(
                                        "Target",
                                        new Microsoft.Xrm.Sdk.Entity()
                                    )
                    };
                }

                return postentityimages;
            }
            set
            {
                postentityimages = value;
            }
        }

        public Guid PrimaryEntityId => Guid.NewGuid();

        public string PrimaryEntityName => "bnb_mockentity";

        public Guid? RequestId => Guid.NewGuid();

        public string SecondaryEntityName => "none";

        public ParameterCollection SharedVariables => new ParameterCollection();

        public Guid BusinessUnitId => Guid.NewGuid();

        public int Stage => 20;

        public string TraceInfo => null;

        public IPluginExecutionContext ParentContext => null;


    }

    class TracingSrvice : ITracingService
    {
        public void Trace(string format, params object[] args)
        {
        }
    }
    public class MemoryServiceProvider : IServiceProvider
    {

        private System.Collections.Generic.Dictionary<System.Type, object> _serviceProviderLookup;

        public MemoryServiceProvider(Entity entity)
        {
            _serviceProviderLookup = new Dictionary<Type, object>();

            _serviceProviderLookup.Add(typeof(IPluginExecutionContext), new MemoryPluginExecutionContext()
            {
                InputParameters = new ParameterCollection {
                                        new KeyValuePair<string, object>(
                                            "Target",
                                            entity
                                        ),
                                        new KeyValuePair<string, object>(
                                            "ConcurrencyBehavior",
                                            Microsoft.Xrm.Sdk.ConcurrencyBehavior.Default
                                        )
                                },

                PreEntityImages = new EntityImageCollection {
                                        new KeyValuePair<string, Entity> (
                                            "Target",
                                            entity
                                        )
                                },

                PostEntityImages = new EntityImageCollection {
                                        new KeyValuePair<string, Entity> (
                                            "Target",
                                            entity
                                        )
                                }
            });
            _serviceProviderLookup.Add(typeof(ITracingService), new TracingSrvice());
            _serviceProviderLookup.Add(typeof(IServiceEndpointNotificationService), new ServiceEndpointNotificationService());
            _serviceProviderLookup.Add(typeof(IOrganizationServiceFactory), new OrganizationServiceFactory());
        }

        public object GetService(Type serviceType)
        {
            if (_serviceProviderLookup.ContainsKey(serviceType))
            {
                object obj;
                if (_serviceProviderLookup.TryGetValue(serviceType, out obj))
                    return obj;
            }
            return null;
        }


    }
}
