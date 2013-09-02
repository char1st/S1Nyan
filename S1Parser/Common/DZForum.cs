﻿// Json Mapping Automatically Generated By JsonToolkit Library for C#
// Diego Trinciarelli 2011
// To use this code you will need to reference Newtonsoft's Json Parser, downloadable from codeplex.
// http://json.codeplex.com/
// 
using System;
using Newtonsoft.Json;

namespace S1Parser
{
    public class DZForum
    {

        public string Version;
        public string Charset;
        public ForumVariables Variables;

        //Empty Constructor
        public DZForum() { }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static DZForum FromJson(string json)
        {
            return JsonConvert.DeserializeObject<DZForum>(json);
        }
    }

    public class Forum
    {

        public string Fid;
        public string Rules;
        public string Fup;
        public string Name;
        public int Threads;
        public int Posts;
        public string Autoclose;
        public string Password;

        //Empty Constructor
        public Forum() { }

    }

    public class Group
    {

        public string Groupid;
        public string Grouptitle;

        //Empty Constructor
        public Group() { }

    }

    public class Forum_threadlist
    {

        public string Tid;
        public string Readperm;
        public string Author;
        public string Authorid;
        public string Subject;
        public string Dateline;
        public string Lastpost;
        public string Lastposter;
        public string Views;
        public string Replies;
        public string Digest;
        public string Attachment;
        public int Dbdateline;
        public int Dblastpost;

        //Empty Constructor
        public Forum_threadlist() { }

    }

    public class Sublist
    {

        public string Fid;
        public string Name;
        public string Threads;
        public string Posts;
        public string Todayposts;

        //Empty Constructor
        public Sublist() { }

    }
/*
    public class Threadtypes
    {

        public string Required;
        public string Listable;
        public string Prefix;
        public Types Types;
        public Icons Icons;
        public Moderators Moderators;

        //Empty Constructor
        public Threadtypes() { }

    }
*/
    public class ForumVariables
    {

        public string Cookiepre;
        public object Auth;
        public string Saltkey;
        public string Member_uid;
        public string Member_username;
        public string Groupid;
        public string Formhash;
        public string Ismoderator;
        public string Readaccess;
        public Forum Forum;
        public Group Group;
        public Forum_threadlist[] Forum_threadlist;
        public Sublist[] Sublist;
        public int Tpp;
        public int Page;
//        public Threadtypes Threadtypes;

        //Empty Constructor
        public ForumVariables() { }

    }

}
//Json Mapping End

