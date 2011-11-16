using System;
using Google.GData.Client;
using Google.GData.Blogger;
using System.Collections.Generic;

namespace Client
{
	public class PostPublisher
	{
		private BloggerService service;
		private string postUrl; 
		public PostPublisher (string userName, string password, string blogPostUrl)
		{
            service = new BloggerService("BloggerSampleApp.NET");

            if (userName != null && userName.Length > 0)
            {
                service.Credentials = new GDataCredentials(userName, password);
            }
			
			postUrl = blogPostUrl;
		}
		
		public void Publish(IEnumerable<BloggerEntry> posts)
        {
			foreach(BloggerEntry post in posts)
			{
				Publish(post);
			}
        }
		
		public void Publish(BloggerEntry post)
		{
			service.Insert(new Uri(postUrl), post);
		}
	}
}

