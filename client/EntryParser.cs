using System;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Google.GData.Blogger;
using Google.GData.Client;
using System.IO;

namespace Client
{
	public class EntryParser
	{
		public List<BloggerEntry> Entries { get; set; }
		public EntryParser (string filename)
		{
			Entries = new List<BloggerEntry>();
			string text = File.ReadAllText(filename);
			JavaScriptSerializer jss = new JavaScriptSerializer();
			jss.MaxJsonLength = Int32.MaxValue;
			Dictionary<string, object> jsonFeed = jss.DeserializeObject(text).GetDict();
			object[] items = jsonFeed["items"].GetArray();
			foreach(object item in items)
			{
				Entries.Add(Parse(item.GetDict()));	
			}
		}
		
		public BloggerEntry Parse(Dictionary<string, object> sharedItem)
		{
			
			BloggerEntry entry = new BloggerEntry();
			
			string postBody = @"link -> <a href=""{0}"">{1}</a>
                                <div></div>
                                <div><blockquote>{2}</blockquote></div>
                                <div></div>
                                <div>{3}</div>";
			
			string url = sharedItem["alternate"].GetArray()[0].GetDict()["href"].GetString();
			string title = url;
			if (sharedItem.ContainsKey("title"))
			{
				title = HttpUtility.HtmlDecode(sharedItem["title"].GetString());
			}
			string quote = "";
			if (sharedItem.ContainsKey("content"))
			{
				quote = HttpUtility.HtmlDecode(sharedItem["content"].GetDict()["content"].GetString());
			}
			string comment = "";
			object[] annotations = sharedItem["annotations"].GetArray();
			if (annotations.Length > 0)
			{
				comment = HttpUtility.HtmlDecode(annotations[0].GetDict()["content"].GetString());
			}
			
            entry.Content.Content = String.Format(postBody, url, title, quote, comment);
            entry.Content.Type = "html"; 
            entry.Title.Text = title;
			entry.Published = sharedItem["published"].GetDateTime();
			
			return entry;
		}
	}
	
	public static class JSONExtensions
	{
		public static string GetString(this object obj)
        {
            return obj as string;
        }
		
		public static object[] GetArray(this object obj)
        {
            return obj as object[];
        }
		
		public static Dictionary<string, object> GetDict(this object obj)
        {
            return obj as Dictionary<string, object>;
        }
		
		public static DateTime GetDateTime(this object obj)
		{
			int secsSinceEpoch = (int)obj;
			long ticksFromBeginCenturyToEpoch = 621355968000000000;
			DateTime epoch = new DateTime(ticksFromBeginCenturyToEpoch);
			long ticksSinceEpoch = Convert.ToInt64(secsSinceEpoch) * 10000000L;
			long ticksTillDateTime = ticksFromBeginCenturyToEpoch + ticksSinceEpoch;
			DateTime newDate = new DateTime(ticksTillDateTime);
			return newDate;
		}
	}
}

