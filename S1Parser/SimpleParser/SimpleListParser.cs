﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S1Parser.SimpleParser
{
    public class SimpleListParser : DataParser<List<S1ListItem>>
    {
        public SimpleListParser() { }
        public SimpleListParser(Stream s) : base(s) { }
        public SimpleListParser(string s) : base(s) { }
        protected override void ParseImpl()
        {
            try
            {
                var root = HtmlPage.FindElements("table").Last().FindFirst("td");
                S1ListItem lastitem = null;
                foreach (var e in root.Descendants())
                {
                    if (e.Name == "li")
                    {
                        var header = e.Element();
                        if (header.Name == "h2")
                        {
                            lastitem = new S1ListItem { Title = header.InnerHtml };
                            theData.Add(lastitem);
                        }
                        else
                        {   //missing ul
                            AddChildItem(lastitem, e);
                        }
                    }
                    else AddChildren(lastitem, e);
                }
            }
            catch (System.Exception) { }
            finally
            {
                if (theData.Count == 0)
                {
                    S1Parser.User.ErrorParser.Parse(HtmlPage);
                    throw new InvalidDataException();
                }
            }
        }

        protected static void AddChildren(S1ListItem item, HtmlElement e)
        {
            var lastitem = item;
            foreach (var ee in e.Descendants())
            {
                if (ee.Name == "li")
                    lastitem = AddChildItem(item, ee);
                if (ee.Name == "ul")
                    AddChildren(lastitem, ee);
            }
        }

        protected static S1ListItem AddChildItem(S1ListItem item, HtmlElement e)
        {
            var result = item;
            if (item != null && (result = S1ListItem.GetItem(e)) != null)
            {
                item.Add(result);
            }
            return result;
        }

    }
}
