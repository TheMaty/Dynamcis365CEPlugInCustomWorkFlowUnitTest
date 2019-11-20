using BnBTechnologies.Xrm.MemoryService;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BnBTechnologies.Xrm
{
    class InitializePluginWorkflow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostOperationbnb_bookingCreate"/> class.
        /// </summary>
        /// <param name="unsecure">Contains public (unsecured) configuration information.</param>
        /// <param name="secure">Contains non-public (secured) configuration information. 
        /// When using Microsoft Dynamics 365 for Outlook with Offline Access, 
        /// the secure string is not passed to a plug-in that executes while the client is offline.</param>
        public InitializePluginWorkflow(string unsecure, string secure)
        {

            // TODO: Implement your custom configuration handling.
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InitializePluginWorkflow()
        {

            // TODO: Implement your custom configuration handling.
        }

        private System.Collections.Generic.List<Entity> memoryDatabaseData;

        /// <summary>
        /// triggerred event as if Dynamics CRM
        /// </summary>
        /// <param name="entity"> Entity lick filling fields in CRM UI</param>
        /// <param name="stepName">Full Name of Step - you can get it from PlugIn Registration tool easily - Look at Event Handler section</param>
        /// <param name="preImage"> Entity shot before database operation </param>
        /// <param name="postImage"> Entity shot after database operation </param>
        /// <param name="inputParameters"> The parameters of the request message that triggered the event that caused the plug-in to execute. We send Entity</param>
        /// <param name="outputParameters">The parameters of the response message after the core platform operation has completed. It is entity for us</param>
        public EntityCollection Execute(Entity entity, string stepName, EntityImageCollection preImage = null, EntityImageCollection postImage = null, ParameterCollection inputParameters = null, ParameterCollection outputParameters = null)
        {
            Assembly assembly = Assembly.LoadFrom("TourismPlugins.dll");
            Type type = assembly.GetType(stepName);

            if (type != null)
            {
                MethodInfo methodInfo = type.GetMethod("Execute");

                if (methodInfo != null)
                {
                    object result = null;
                    
                    object classInstance = Activator.CreateInstance(type, new object[] { "", "" });

                    BnBTechnologies.Xrm.MemoryService.Provider serviceProvider = new BnBTechnologies.Xrm.MemoryService.Provider(entity);

                    MemoryPluginExecutionContext executionContext = (MemoryPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                    if (preImage != null)
                        executionContext.PreEntityImages = preImage;

                    if (postImage != null)
                        executionContext.PostEntityImages = postImage;

                    if (inputParameters != null)
                        executionContext.InputParameters = inputParameters;

                    if (outputParameters != null)
                        executionContext.OutputParameters = outputParameters;

                    // Obtain the organization factory service from the service provider.
                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                    //load initial data
                    ((OrganizationService)factory.CreateOrganizationService(null)).MockMemoryDatabase = memoryDatabaseData;


                    object[] parametersArray = new object[] { serviceProvider };
                    result = methodInfo.Invoke(classInstance, parametersArray);


                    // Obtain the organization service reference which you will need for web service calls.  
                    return new EntityCollection(((OrganizationService)factory.CreateOrganizationService(null)).MockMemoryDatabase);
                }
            }

            return null;
        }

        public void LoadSampleDataToMemory(EntityCollection collection)
        {
            if (memoryDatabaseData == null)
            {
                memoryDatabaseData = new List<Entity>();
            }

            foreach (Entity entity in collection.Entities)
            {
                memoryDatabaseData.Add(entity);
            }
        }
    }

}