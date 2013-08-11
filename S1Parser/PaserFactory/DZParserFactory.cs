﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using S1Parser.DZParser;
using S1Parser.SimpleParser;

namespace S1Parser.PaserFactory
{
    public class DZParserFactory : IParserFactory
    {
        internal const int ThreadsPerPage = 30;
        internal const int PostsPerPage = 30;

        public IResourceService ResourceService { get; set; }

        public string Path
        {
            get { return S1Resource.DZMobilePath; }
        }

        public async Task<IList<S1ListItem>> GetMainListData()
        {
            Stream s = await GetMainListStream();
            return new SimpleListParser(s).GetData();
        }

        public async Task<Stream> GetMainListStream()
        {
            Stream s = await ResourceService.GetResourceStream(GetMainUri());
            return s;
        }

        public IList<S1ListItem> ParseMainListData(Stream s)
        {
            return new SimpleListParser(s).GetData();
        }

        public IList<S1ListItem> ParseMainListData(string s)
        {
            return new SimpleListParser(s).GetData();
        }

        public async Task<S1ThreadList> GetThreadListData(string fid, int page)
        {
            Stream s = await ResourceService.GetResourceStream(GetThreadListUri(fid, page));
            return new DZThreadListParser(s).GetData();
        }

        public async Task<S1ThreadPage> GetThreadData(string tid, int page)
        {
            Stream s = await ResourceService.GetResourceStream(GetThreadUri(tid, page));
            return new DZThreadParser(s).GetData();
        }

        protected virtual Uri GetMainUri()
        {
            return new Uri(S1Resource.SimpleBase);
        }

        protected virtual Uri GetThreadListUri(string fid, int page)
        {
            return new Uri(S1Resource.DZMobileBase + string.Format("?module=forumdisplay&fid={0}&page={1}&tpp={2}", fid, page, ThreadsPerPage));
        }

        protected virtual Uri GetThreadUri(string tid, int page)
        {
            return new Uri(S1Resource.DZMobileBase + string.Format("?module=viewthread&tid={0}&page={1}&ppp={2}", tid, page, PostsPerPage));
        }
    }
}