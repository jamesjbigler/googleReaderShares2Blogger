using System;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Google.GData.Client;
using Google.GData.Blogger;
using System.Net;
using System.Linq;

namespace Client
{
    public class Blogger : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label UrlLabel;
        private System.Windows.Forms.Label UserLabel;
		private System.Windows.Forms.Label BlogLabel;
        private System.Windows.Forms.Label PasswordLabel;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Button FileOpen;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private System.Windows.Forms.TextBox BloggerURI;
        private System.Windows.Forms.TreeView FeedView;
        private System.Windows.Forms.Button ImportPosts;
        public System.Windows.Forms.ComboBox FeedChooser;
		private System.Windows.Forms.Label StartLabel;
        private System.Windows.Forms.TextBox StartIndex;
        
		
		
		private OpenFileDialog OpenFileDialog;
		private IEnumerable<BloggerEntry> posts;

        private string feedUri; 
		
        public Blogger()
        {
            InitializeComponent();

            this.feedUri = null; 
		}

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Application.Run(new Blogger());
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BloggerURI = new System.Windows.Forms.TextBox();
            this.UrlLabel = new System.Windows.Forms.Label();
            this.UserLabel = new System.Windows.Forms.Label();
			this.BlogLabel = new System.Windows.Forms.Label();
            this.PasswordLabel = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.FeedView = new System.Windows.Forms.TreeView();
            this.ImportPosts = new System.Windows.Forms.Button();
            this.FileLabel = new System.Windows.Forms.Label();
            this.FileName = new System.Windows.Forms.TextBox();
            this.FileOpen = new System.Windows.Forms.Button();	
			this.StartLabel = new System.Windows.Forms.Label();
            this.StartIndex = new System.Windows.Forms.TextBox();
            
			this.OpenFileDialog = new OpenFileDialog();
			
            this.FeedChooser = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            
			this.UrlLabel.Location = new System.Drawing.Point(8, 16);
            this.UrlLabel.Name = "label1";
            this.UrlLabel.Size = new System.Drawing.Size(64, 16);
            this.UrlLabel.TabIndex = 2;
            this.UrlLabel.Text = "URL:";

			
            this.BloggerURI.Location = new System.Drawing.Point(88, 16);
            this.BloggerURI.Name = "BloggerURI";
            this.BloggerURI.Size = new System.Drawing.Size(296, 20);
            this.BloggerURI.TabIndex = 1;
            this.BloggerURI.Text = "http://www.blogger.com/feeds/default/blogs";
			this.BloggerURI.ReadOnly = true;

			
            this.UserLabel.Location = new System.Drawing.Point(8, 48);
            this.UserLabel.Name = "label2";
            this.UserLabel.Size = new System.Drawing.Size(64, 24);
            this.UserLabel.TabIndex = 3;
            this.UserLabel.Text = "User:";

			
            this.PasswordLabel.Location = new System.Drawing.Point(8, 80);
            this.PasswordLabel.Name = "label3";
            this.PasswordLabel.Size = new System.Drawing.Size(64, 16);
            this.PasswordLabel.TabIndex = 4;
            this.PasswordLabel.Text = "Password";
			
			this.StartLabel.Location = new System.Drawing.Point(8, 144);  //144
            this.StartLabel.Name = "label4";
            this.StartLabel.Size = new System.Drawing.Size(64, 16);
            this.StartLabel.TabIndex = 4;
            this.StartLabel.Text = "Skip To Post";

            this.StartIndex.Location = new System.Drawing.Point(88, 144);
            this.StartIndex.Name = "StartIndex";
            this.StartIndex.Size = new System.Drawing.Size(296, 20);
            this.StartIndex.TabIndex = 6;
            this.StartIndex.Text = "0";
			
            // 
            // UserName
            // 
            this.UserName.Location = new System.Drawing.Point(88, 48);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(296, 20);
            this.UserName.TabIndex = 5;
            this.UserName.Text = "";
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(88, 80);
            this.Password.Name = "Password";
            this.Password.PasswordChar = '*';
            this.Password.Size = new System.Drawing.Size(296, 20);
            this.Password.TabIndex = 6;
            this.Password.Text = "";

			this.FileLabel.Location = new System.Drawing.Point(8, 176);
            this.FileLabel.Name = "label4";
            this.FileLabel.Size = new System.Drawing.Size(64, 16);
            this.FileLabel.TabIndex = 4;
            this.FileLabel.Text = "File";
			
            this.FileName.Location = new System.Drawing.Point(88, 176); //176
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(296, 20);
            this.FileName.TabIndex = 6;
            this.FileName.ReadOnly = true;	
			this.FileName.Text = "";
			
            this.FileOpen.Location = new System.Drawing.Point(390, 176);
            this.FileOpen.Name = "Open";
            this.FileOpen.Size = new System.Drawing.Size(84, 20);
            this.FileOpen.TabIndex = 7;
            this.FileOpen.Text = "&Open";
            this.FileOpen.Click += new System.EventHandler(this.Open_Click);

            // 
            // FeedView
            // 
            this.FeedView.ImageIndex = -1;
            this.FeedView.Location = new System.Drawing.Point(16, 208);
            this.FeedView.Name = "FeedView";
            this.FeedView.SelectedImageIndex = -1;
            this.FeedView.Size = new System.Drawing.Size(544, 280);
            this.FeedView.TabIndex = 8;
            // 
            // AddEntry
            // 
            this.ImportPosts.Enabled = false;
            this.ImportPosts.Location = new System.Drawing.Point(16, 517);
            this.ImportPosts.Name = "AddEntry";
            this.ImportPosts.Size = new System.Drawing.Size(104, 24);
            this.ImportPosts.TabIndex = 9;
            this.ImportPosts.Text = "&Import...";
            this.ImportPosts.Click += new System.EventHandler(this.ImportPosts_Click);

			
            this.BlogLabel.Location = new System.Drawing.Point(8, 112);
            this.BlogLabel.Name = "label2";
            this.BlogLabel.Size = new System.Drawing.Size(64, 24);
            this.BlogLabel.TabIndex = 3;
            this.BlogLabel.Text = "Blog:";
			
            this.FeedChooser.Location = new System.Drawing.Point(88, 112);  //112
            this.FeedChooser.Name = "FeedChooser";
            this.FeedChooser.Size = new System.Drawing.Size(296, 21);
            this.FeedChooser.TabIndex = 10;
            this.FeedChooser.Text = "choose your blog.... ";
			this.FeedChooser.Click += new System.EventHandler(this.FeedChooser_Click);
            this.FeedChooser.SelectedIndexChanged += new System.EventHandler(this.FeedChooser_SelectedIndexChanged);
            // 
            // Blogger
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(592, 570);
            this.Controls.Add(this.FeedChooser);
			this.Controls.Add(this.BlogLabel);
            this.Controls.Add(this.ImportPosts);
            this.Controls.Add(this.FeedView);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.UserName);
            this.Controls.Add(this.PasswordLabel);
            this.Controls.Add(this.UserLabel);
            this.Controls.Add(this.UrlLabel);
            this.Controls.Add(this.BloggerURI);
			this.Controls.Add(this.FileLabel);
			this.Controls.Add(this.FileName);
			this.Controls.Add(this.FileOpen);
			this.Controls.Add(this.StartLabel);
			this.Controls.Add(this.StartIndex);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Name = "Shares 2 Blogger";
            this.Text = "Google Reader Shares 2 Blogger";
            this.ResumeLayout(false);

        }
        #endregion

	    private void Open_Click(object sender, System.EventArgs e)
        {
            DialogResult result = this.OpenFileDialog.ShowDialog();
		    if (result == DialogResult.OK)
		    {
		        FileName.Text = OpenFileDialog.FileName;
				RefreshPosts();
		    } 
        }	

        private void FeedChooser_Click(object sender, System.EventArgs e)
		{
			if (this.FeedChooser.Items.Count == 0)
			{
            	RefreshFeedList(); 
			}
		}

        private void RefreshFeedList()
        {
            string bloggerURI  = this.BloggerURI.Text;
            string userName =    this.UserName.Text;
            string passWord =    this.Password.Text;

            BloggerQuery query = new BloggerQuery();
            BloggerService service = new BloggerService("BloggerSampleApp.NET");

            if (userName != null && userName.Length > 0)
            {
                service.Credentials = new GDataCredentials(userName, passWord);
            }

            // only get event's for today - 1 month until today + 1 year

            query.Uri = new Uri(bloggerURI);

            Cursor.Current = Cursors.WaitCursor; 

            // start repainting
            this.FeedChooser.BeginUpdate(); 

            BloggerFeed bloggerFeed = service.Query(query);
            // Display a wait cursor while the TreeNodes are being created.

            this.FeedChooser.DisplayMember = "Title"; 

            while (bloggerFeed != null && bloggerFeed.Entries.Count > 0)
            {
                foreach (BloggerEntry entry in bloggerFeed.Entries) 
                {
                    int iIndex = this.FeedChooser.Items.Add(new ListEntry(entry)); 
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

            if (this.FeedChooser.Items.Count > 0) 
            {
                this.FeedChooser.Items.Insert(0, "Choose the feed..."); 
            }
            else 
            {
                 this.FeedChooser.Items.Insert(0, "No feeds for this user..."); 
            }
            
            
            // Reset the cursor to the default for all controls.
            Cursor.Current = Cursors.Default;
            // End repainting the combobox
            this.FeedChooser.EndUpdate();
        
            
        }



        private void RefreshPosts() 
        {
            // Suppress repainting the TreeView until all the objects have been created.
            this.FeedView.BeginUpdate(); 
            // Clear the TreeView each time the method is called.
            this.FeedView.Nodes.Clear(); 

            // set wait cursor before...
            Cursor.Current = Cursors.WaitCursor; 
            
			EntryParser parser = new EntryParser(this.FileName.Text);
			
			int startIndex = String.IsNullOrEmpty(StartIndex.Text) ? 0 : Convert.ToInt32(StartIndex.Text);
			this.posts = parser.Entries.Skip(startIndex).Take(50);
			
            foreach (BloggerEntry entry in posts) 
            {
                int iIndex = this.FeedView.Nodes.Add(new TreeNode(entry.Title.Text)); 
                if (iIndex >= 0) 
                {
                    this.FeedView.Nodes[iIndex].Nodes.Add(new TreeNode("published: " + entry.Published.ToString())); 
                    if (entry.Content.Content.Length > 50) 
                    {
                        this.FeedView.Nodes[iIndex].Nodes.Add(new TreeNode(entry.Content.Content.Substring(0, 50)+"....")); 
                    } 
                    else 
                    {
                        this.FeedView.Nodes[iIndex].Nodes.Add(new TreeNode(entry.Content.Content)); 
                    }

                }
            }

            // Reset the cursor to the default for all controls.
            Cursor.Current = Cursors.Default;
            // stop repainting the TreeView.
            this.FeedView.EndUpdate();
			
			this.ImportPosts.Enabled = true; 

        }

        private void ImportPosts_Click(object sender, System.EventArgs e)
        {
            string userName =    this.UserName.Text;
			string password =    this.Password.Text;
			PostPublisher publisher = new PostPublisher(userName, password, this.feedUri);
			
			publisher.Publish(posts);
        }

        /// <summary>
        /// get's called when a new feed was choosen from the combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FeedChooser_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ListEntry listEntry = this.FeedChooser.SelectedItem as ListEntry; 

            this.feedUri = null; 

            if (listEntry != null) 
            {
                BloggerEntry entry = listEntry.Entry; 

                if (entry != null) 
                {
                    // find the link.rel==feed uri and refresh the treeview
                    foreach (AtomLink link in entry.Links) 
                    {
                        if (link.Rel == BaseNameTable.ServicePost) 
						{
                            this.feedUri = link.HRef.ToString(); 
                            break;
                        }
                    }
				}
            }
        }
    }


    /// <summary>
    /// little helper class to put an atomentry into a combobox
    /// </summary>
    public class ListEntry 
    {
        private BloggerEntry entry;

        public BloggerEntry Entry 
        {
            get 
            {
                return this.entry;
            }
            set 
            {
                this.entry = value;
            }
        }

        public ListEntry(BloggerEntry entry)
        {
            this.entry = entry; 
        }

        public override string ToString() 
        {
            return this.entry != null ? this.entry.Title.Text : "No Entry"; 
        }
    }


}
