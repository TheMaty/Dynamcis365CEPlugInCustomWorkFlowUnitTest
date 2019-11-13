using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Microsoft.Xrm.Sdk;

namespace TourimPlugins___UnitTestProject
{
    /// <summary>
    /// Summary description for BookingNameCreationTest
    /// </summary>
    [TestClass]
    public class BookingNameCreationTest
    {
        public BookingNameCreationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void BookingName()
        {
            InitializePlugIn plgIn = new InitializePlugIn("", "");

            Entity preEntity = new Entity("bnb_booking");

            preEntity.Id = Guid.NewGuid();
            preEntity.Attributes.Add("bnb_bookingid", Guid.NewGuid());
            //entityNew.Attributes.Add("bnb_name", "Booking Unit Test");

            EntityReference refEntity = new EntityReference("bnb_tour");
            refEntity.Id = Guid.NewGuid();
            refEntity.Name = "Tour Unit Test";
            preEntity.Attributes.Add("bnb_tourid", refEntity);
            
            refEntity = new EntityReference("bnb_accommodation");
            refEntity.Id = Guid.NewGuid();
            refEntity.Name = "Accommodation Unit Test";
            preEntity.Attributes.Add("bnb_accommodationid", refEntity);

            preEntity.Attributes.Add("bnb_checkin", DateTime.Now);
            preEntity.Attributes.Add("bnb_checkout", DateTime.Now);

            EntityCollection postEntityColl = plgIn.Execute(preEntity, "TourismPlugins.PostOperationbnb_bookingCreate");

            foreach (Entity postEntity in postEntityColl.Entities)
            {
                if (postEntity.Contains("bnb_name"))
                    Assert.AreEqual(getName(preEntity), postEntity.Attributes["bnb_name"]);
                else
                    Assert.Fail("return entity does not contain bnb_name field");
            }


        }

        private string getName(Entity entity)
        {
            return "<CRM>"
                    + (entity.Contains("bnb_tourid") ? (" Tour:" + ((EntityReference)entity["bnb_tourid"]).Name).Replace("<CRM>", "") : "")
                    + (entity.Contains("bnb_accommodationid") ? ((((EntityReference)entity["bnb_accommodationid"]).Name == null ? "" : (" - Accommodation:" + ((EntityReference)entity["bnb_accommodationid"]).Name))) : "")
                    + (entity.Contains("bnb_checkin") ? (" " + Convert.ToDateTime(entity["bnb_checkin"].ToString()).ToString("yyyy/MM/dd")) : "")
                    + (entity.Contains("bnb_checkout") ? (" - " + Convert.ToDateTime(entity["bnb_checkout"].ToString()).ToString("yyyy/MM/dd")) : "");
        }
    }
}
