using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Newtonsoft.Json;
using RTMSharp;

namespace RTM_Tests
{
    [TestClass]
    public class UnitTest1
    {
        string locationList_json;
        string taskList_json;
        string lists_json;

        [TestInitialize]
        public void LoadData()
        {
            StreamReader sr = new StreamReader(@"SampleData\getTaskList.json");
            taskList_json = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader(@"SampleData\getLists.json");
            lists_json = sr.ReadToEnd();
            sr.Close();

            sr = new StreamReader(@"SampleData\getLocationList.json");
            locationList_json = sr.ReadToEnd();
            sr.Close();

        }

        [TestMethod]
        public void ConfirmDataLoaded()
        {
            Assert.IsTrue(locationList_json.Length > 0);
            Assert.IsTrue(taskList_json.Length > 0);
            Assert.IsTrue(lists_json.Length > 0);
        }

        [TestMethod]
        public void ParseLists()
        {
            RootObject Lists = JsonConvert.DeserializeObject<RootObject>(lists_json);
            Assert.IsTrue(Lists.isValid());
        }

        [TestMethod]
        public void ParseLocations()
        {
            RootObject Locations = JsonConvert.DeserializeObject<RootObject>(locationList_json);
            Assert.IsTrue(Locations.isValid());
        }

        [TestMethod]
        public void ParseTaskList()
        {
            RootObject Tasks = JsonConvert.DeserializeObject<RootObject>(taskList_json);
            Assert.IsTrue(Tasks.isValid());
        }

        [TestCleanup]
        public void CleanUp()
        {
            //nothing needed here right now
        }
    }
}
