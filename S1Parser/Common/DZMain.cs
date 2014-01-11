﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace S1Parser
{

    public class ThreadType
    {

        [JsonProperty("required")]
        public string Required { get; set; }

        [JsonProperty("types")]
        public Dictionary<int, string> Types { get; set; }
    }

    public enum ForumTypes
    {
        Group,
        Forum,
        Sub
    }

    public class ForumItem
    {

        [JsonProperty("fid")]
        public string Fid { get; set; }

        [JsonProperty("type")]
        public ForumTypes Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("fup")]
        public string Fup { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("threadtypes")]
        public ThreadType Threadtypes { get; set; }

        [JsonProperty("postperm")]
        public string Postperm { get; set; }
    }

    public class Variables
    {

        [JsonProperty("cookiepre")]
        public string Cookiepre { get; set; }

        [JsonProperty("auth")]
        public object Auth { get; set; }

        [JsonProperty("saltkey")]
        public string Saltkey { get; set; }

        [JsonProperty("member_uid")]
        public string MemberUid { get; set; }

        [JsonProperty("member_username")]
        public string MemberUsername { get; set; }

        [JsonProperty("groupid")]
        public string Groupid { get; set; }

        [JsonProperty("formhash")]
        public string Formhash { get; set; }

        [JsonProperty("ismoderator")]
        public object Ismoderator { get; set; }

        [JsonProperty("readaccess")]
        public string Readaccess { get; set; }

        [JsonProperty("forums")]
        public ForumItem[] Forums { get; set; }
    }

    public class DZMain
    {

        [JsonProperty("Version")]
        public string Version { get; set; }

        [JsonProperty("Charset")]
        public string Charset { get; set; }

        [JsonProperty("Variables")]
        public Variables Variables { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static DZMain FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DZMain>(json);
        }
    }

}
