using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Newtonsoft.Json;
using RTMSharp;
using System.Collections.Generic;

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
            RootObject ro = JsonConvert.DeserializeObject<RootObject>(lists_json);
            Assert.IsTrue(ro.isValid());
            RTMLists Lists = ro.getLists();
            Assert.IsTrue(Lists.allLists.Count == 22);
            Assert.IsTrue(Lists.getListByName("Inbox").id == "11001183");
            Assert.IsTrue(Lists.getListById("11001187").name == "Sent");
            Assert.IsTrue(Lists.getListByName(null) == null);
            Assert.IsTrue(Lists.getListById(null) == null);
            Assert.IsTrue(Lists.getListByName("") == null);
            Assert.IsTrue(Lists.getListById("") == null);
        }
        
        [TestMethod]
        public void ParseLocations()
        {
            RootObject ro = JsonConvert.DeserializeObject<RootObject>(locationList_json);
            Assert.IsTrue(ro.isValid());
            List<RTMLocation> Locations = ro.getLocations();
            Assert.IsTrue(Locations.Count == 7);
            
        }

        [TestMethod]
        public void ParseTaskList()
        {
            RootObject Tasks = JsonConvert.DeserializeObject<RootObject>(taskList_json);
            Assert.IsTrue(Tasks.isValid());
            
        }

        [TestMethod]
        public void ParseTasksandTestLocations()
        {
            RootObject ro = JsonConvert.DeserializeObject<RootObject>(taskList_json);
            Assert.IsTrue(ro.isValid());
            List<RTMLocation> Locations = ro.getLocations();
            Assert.IsTrue(Locations == null);
        }

        [TestMethod]
        public void TasksWithEstimates()
        {
            RootObject Tasks = JsonConvert.DeserializeObject<RootObject>(taskList_json);
            List<Tasksery> _tasksWithEstimates = new List<Tasksery>();
            
        }

        [TestCleanup]
        public void CleanUp()
        {
            //nothing needed here right now
        }
    }
}
