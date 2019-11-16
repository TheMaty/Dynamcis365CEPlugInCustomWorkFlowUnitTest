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
        /// <summary>
        /// triggerred event as if Dynamics CRM
        /// </summary>
        /// <param name="entity"> Entity lick filling fields in CRM UI</param>
        /// <param name="stepName">Full Name of Step - you can get it from PlugIn Registration tool easily - Look at Event Handler section</param>
        public EntityCollection Execute(Entity entity, string stepName, EntityImageCollection preImage = null, EntityImageCollection postImage = null)
        {
            Assembly assembly = Assembly.LoadFrom("TourismPlugins.dll");
            Type type = assembly.GetType(stepName);

            if (type != null)
            {
                MethodInfo methodInfo = type.GetMethod("Execute");

                if (methodInfo != null)
                {
                    object result = null;
                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    object classInstance = Activator.CreateInstance(type, new object[] { "", "" });

                    BnBTechnologies.Xrm.MemoryService.Provider serviceProvider = new BnBTechnologies.Xrm.MemoryService.Provider(entity);

                    MemoryPluginExecutionContext executionContext = (MemoryPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                    if (preImage != null)
                        executionContext.PreEntityImages = preImage;

                    if (postImage != null)
                        executionContext.PostEntityImages = postImage;

                    object[] parametersArray = new object[] { serviceProvider };
                    result = methodInfo.Invoke(classInstance, parametersArray);

                    // Obtain the organization factory service from the service provider.
                    IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                    // Obtain the organization service reference which you will need for web service calls.  
                    return ((OrganizationService)factory.CreateOrganizationService(null)).postEntityColl;
                }
            }

            return null;
        }
    }

}