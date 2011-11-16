using System;
using NDesk.Options;
using Client;
using System.Threading;
using System.Linq;
using Google.GData.Client;
using Google.GData.Blogger;

namespace Console
{
	class Configuration
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string BlogTitle { get; set; }
		public string FileName { get; set; }
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			Configuration configuration = GetConfiguration (args);
			BlogLocator blogLocator = new BlogLocator(configuration.UserName, configuration.Password);
			EntryParser entryParser = new EntryParser(configuration.FileName);
			string blogPostUrl = blogLocator.FindBlogByTitle(configuration.BlogTitle).PostUri.Content;
			PostPublisher postPublisher = new PostPublisher(configuration.UserName, configuration.Password, blogPostUrl);
			
			ImportPosition importPosition = new ImportPosition();
			Thread.Sleep(importPosition.TimeToNextImport);
			
			var blogEntries = entryParser.Entries.Skip(importPosition.BlogIndex).Take(50);
			try
			{
				foreach(var blogEntry in blogEntries)
				{
					postPublisher.Publish(blogEntry);
					importPosition++;
				}
			} catch(Exception e)
			{
				System.Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
			}
			
			importPosition.Save();
		}

		static Configuration GetConfiguration (string[] args)
		{
			Configuration configuration = new Configuration();
			bool help = false;
			OptionSet option_set = new OptionSet ()  
			    .Add ("?|help|h", "Prints out the options.", option => help = option != null)  
			    .Add ("u=|username=", 
			       "REQUIRED: User Name - The username for your blogger account.", 
			       option => configuration.UserName = option)
				.Add ("p=|password=", 
			       "REQUIRED: Password - The password for your blogger account.", 
			       option => configuration.Password = option)
				.Add ("b=|blogtitle=", 
			       "REQUIRED: Blog Title - The title for your blog.", 
			       option => configuration.BlogTitle = option)
				.Add ("f=|filename=", 
			       "REQUIRED: File Name - The file containing your shared items downloaded from Reader.", 
			       option => configuration.FileName = option)
					;
			try {
				option_set.Parse (args);
			} catch (OptionException) {
				show_help ("Error - usage is:", option_set);
			}
						
						
			if (help) {
				const string usage_message = 
			        "console.exe /u[sername] VALUE /p[assword] VALUE " +
			        "/b[logtitle] VALUE /f[ilename] Value";
				show_help (usage_message, option_set);
			}
						
						
			if (configuration.UserName == null) {
				show_help ("Error: You must specify a User Name (/u).", option_set);
			}
			
			if (configuration.Password == null) {
				show_help ("Error: You must specify a Password (/p).", option_set);
			}
			
			if (configuration.BlogTitle == null) {
				show_help ("Error: You must specify a Blog Title (/u).", option_set);
			}
			
			if (configuration.FileName == null) {
				show_help ("Error: You must specify a File Name (/u).", option_set);
			}
			return configuration;
		}
		
		public static void show_help (string message, OptionSet option_set)
		{
			System.Console.Error.WriteLine (message);
			option_set.WriteOptionDescriptions (System.Console.Error);
			Environment.Exit (-1);
		}
	}
}
