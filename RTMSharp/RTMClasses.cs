using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RTMSharp
{
    /// <summary>
    /// Converter class to deal with RTM json API issue = https://groups.google.com/d/topic/rememberthemilk-api/aNegBdRtw5E/discussion
    /// Code from http://fisenkodv.com/custom-jsonconverter-to-fix-bad-json-results/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class SingleValueArrayConverter<T> : JsonConverter
    {      
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object retVal = new Object();
            if (reader.TokenType == JsonToken.StartObject || 
                reader.TokenType == JsonToken.String)
            {
                T instance = (T)serializer.Deserialize(reader, typeof(T));
                retVal = new List<T>() { instance };
            }
            else if (reader.TokenType == JsonToken.StartArray)
            {
                retVal = serializer.Deserialize(reader, objectType);
            }            
            return retVal;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
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
        public List<RTMLocation> getLocations()
        {
            if (this.rsp!= null &&
                this.rsp.locations !=null &&
                this.rsp.locations.location != null)
            {
                return this.rsp.locations.location;
            } else
            {
                return null;
            }            
        }

        

        public RTMLists getLists()
        {
            if (this.rsp != null &&
                this.rsp.lists != null)
            {
                return this.rsp.lists;
            }else
            {
                return null;
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
        }
    }

    public class RTMTasks
    {
        public string rev { get; set; }
        public List<RTMList> list { get; set; }
    }

    public class RTMLists
    {
       
        public RTMList getListByName(string listName)
        {
            
            if (this.allLists != null)
            {
                return this.allLists.Where(l => l.name == listName)
                                .FirstOrDefault<RTMList>();

            }
            else
            {
                return null;
            }

        }
        public RTMList getListById(string listId)
        {
            if (this.allLists != null)
            {
                return this.allLists.Where(l => l.id == listId)
                            .FirstOrDefault<RTMList>();
            }
            else
            {
                return null;
            }
        }
        [JsonProperty(PropertyName = "list")]
        public List<RTMList> allLists { get; set; }
    }

    public class RTMList
    {
        public override string ToString()
        {
            return string.Format("List ID: {0} - ({1} items)", new object[] { this.id, this.taskseries.Count });
        }
        public string id { get; set; }
        public string name { get; set; }
        public string deleted { get; set; }
        public string locked { get; set; }
        public string archived { get; set; }
        public string position { get; set; }
        public string smart { get; set; }
        public string sort_order { get; set; }
        public string filter { get; set; }

        // TaskSeries could be an array or a single item so we need a custom converter
        //[JsonConverter(typeof(taskSeriesConverter))]
        [JsonConverter(typeof(SingleValueArrayConverter<Tasksery>))]
        public List<Tasksery> taskseries { get; set; }

       
    }

    public class Tasksery
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string url { get; set; }
        public string location_id { get; set; }
        public Tags tags { get; set; }

        public List<object> participants { get; set; }
        public List<object> notes { get; set; }

        //custom converter as this could be an array of tasks or a single task
        //[JsonConverter(typeof(taskConverter))] 
        [JsonConverter(typeof(SingleValueArrayConverter<RTMTask>))]
        public List<RTMTask> task { get; set; } 
        public Rrule rrule { get; set; }

        public override string ToString()
        {
            return string.Format("TaskSeries ID: {0}",this.id);
        }
    }
    //public class taskConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {

    //        List<RTMTask> _taskList = new List<RTMTask>();

    //        if (reader.TokenType == JsonToken.StartObject)
    //        {
    //            JObject jsonObject = JObject.Load(reader); //this will fail if the data passed in is an array

    //            _taskList.Add(jsonObject.ToObject<RTMTask>());

    //        }
    //        else if (reader.TokenType == JsonToken.StartArray)
    //        {
    //            JArray jsonArray = JArray.Load(reader);

    //            foreach (JObject jsonObject in jsonArray.ToList())
    //            {
    //                if (jsonObject !=null)
    //                {
    //                    _taskList.Add(jsonObject.ToObject<RTMTask>());
    //                }                   
    //            }
                
    //            //throw new NotImplementedException();
    //        }

    //        System.Diagnostics.Debug.WriteLine(_taskList.FirstOrDefault<RTMTask>().id);
    //        return _taskList;
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

   
    //public class tagConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type objectType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        List<string> _tags = new List<string>();

    //        if (reader.TokenType == JsonToken.String)
    //        {
    //            object val = reader.Value;
    //            if (val != null)
    //            {
    //                _tags.Add(val.ToString());
    //            }               
    //        }
    //        else if (reader.TokenType == JsonToken.StartArray)
    //        {
    //            JArray jsonArray = JArray.Load(reader);
    //            foreach (string t in jsonArray.ToList())
    //            {
    //                _tags.Add(t);
    //            }
                                                
    //            //throw new NotImplementedException();
    //        }
    //        else
    //        {                
    //            throw new NotImplementedException();
    //        }

            
    //        return _tags;
        
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();            
    //    }
    //}

    public class Rrule
    {
        public string every { get; set; }

        [JsonProperty(PropertyName = "$t")]
        public string Dollart { get; set; } 
    }

    public class RTMTask
    {
        public string id { get; set; }
        public DateTime? due { get; set; }
        public string has_due_time { get; set; }
        public DateTime added { get; set; }
        public DateTime? completed { get; set; }
        public string deleted { get; set; }
        public string priority { get; set; }
        public string postponed { get; set; }
        public string estimate { get; set; }
        public bool isOverdue()
        {
            if (this.due != null)
            {
                return (this.due < DateTime.Now);
            }else
            {
                return false;
            }
        }
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
        //will need to write a custom converter
        //[JsonConverter(typeof(tagConverter))]
        [JsonConverter(typeof(SingleValueArrayConverter<string>))]
        public List<string> tag { get; set; }
    }

}
