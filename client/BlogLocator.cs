using System;
using System.Collections.Generic;
using Google.GData.Blogger;
using Google.GData.Client;
using System.Linq;

namespace Client
{
	public class BlogLocator
	{
		public const string BloggerFeedUrl = "http://www.blogger.com/feeds/default/blogs";
		public IEnumerable<BloggerEntry> Blogs { get; private set; }
		private string UserName;
		private string Password;
		public BlogLocator (string userName, string password)
		{
			UserName = userName;
			Password = password;
			Blogs = FindBlogs();
		}
		
		private IEnumerable<BloggerEntry> FindBlogs()
		{
			BloggerQuery query = new BloggerQuery();
            BloggerService service = new BloggerService("BloggerSampleApp.NET");

            if (UserName != null && UserName.Length > 0)
            {
                service.Credentials = new GDataCredentials(UserName, Password);
            }

            query.Uri = new Uri(BloggerFeedUrl);


            BloggerFeed bloggerFeed = service.Query(query);
			
			List<BloggerEntry> blogs = new List<BloggerEntry>();

            while (bloggerFeed != null && bloggerFeed.Entries.Count > 0)
            {
                foreach (BloggerEntry entry in bloggerFeed.Entries) 
                {
                    blogs.Add(entry); 
                }
                // do the chunking...
                if (bloggerFeed.NextChunk != null) 
                {
                    query.Uri = new Uri(bloggerFeed.NextChunk); 
                    bloggerFeed = service.Query(query);
                }
                else 
                {
                    bloggerFeed = null; 
                }
            }
			
			return blogs;
		}
		
		public BloggerEntry FindBlogByTitle(string title)
		{
			return Blogs.First(b => b.Title.Text == title);
		}
	}
}

