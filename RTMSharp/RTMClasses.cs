using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTMSharp
{
    public class RootObject
    {
        public Rsp rsp { get; set; }
        public bool isValid()
        {
            if (this.rsp != null)
            {
                return this.rsp.isValid();
            }
            else
            {
                return false;
            }
        }

    }

    public class Rsp
    {
        public string stat { get; set; }
        public RTMLocations locations { get; set; }
        public RTMLists lists { get; set; }
        public RTMTasks tasks { get; set; }

        public bool isValid()
        {
            if (this.stat != null && this.stat.Equals("ok"))
            {
                return true;
            }
            else
            {
                return false;
            }
            //throw new NotImplementedException();
        }
    }

    public class RTMTasks
    {
        public string rev { get; set; }
        public List<RTMList> list { get; set; }
    }

    public class RTMLists
    {
        public List<RTMList> list { get; set; }
    }

    public class RTMList
    {
        public string id { get; set; }
        public string name { get; set; }
        public string deleted { get; set; }
        public string locked { get; set; }
        public string archived { get; set; }
        public string position { get; set; }
        public string smart { get; set; }
        public string sort_order { get; set; }
        public string filter { get; set; }
        public List<Tasksery> taskseries { get; set; }
    }

    public class Tasksery
    {
        public string id { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string location_id { get; set; }
        public Tags tags { get; set; }
        public List<object> participants { get; set; }
        public List<object> notes { get; set; }
        public RTMTask task { get; set; }
        public Rrule rrule { get; set; }
    }

    public class Rrule
    {
        public string every { get; set; }
        public string Dollart { get; set; } //represented in JSON as $t ??? not sure if this will parse correctly
    }

    public class RTMTask
    {
        public string id { get; set; }
        public string due { get; set; }
        public string has_due_time { get; set; }
        public string added { get; set; }
        public string completed { get; set; }
        public string deleted { get; set; }
        public string priority { get; set; }
        public string postponed { get; set; }
        public string estimate { get; set; }
    }
    public class RTMLocations
    {
        public List<RTMLocation> location { get; set; }
    }

    public class RTMLocation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string zoom { get; set; }
        public string address { get; set; }
        public string viewable { get; set; }
    }

    public class Tags
    {
        //Problem here as data is represented both as an array and as an individual tag
        //will need to write a custome converter
        public List<string> tag { get; set; }
    }

}
