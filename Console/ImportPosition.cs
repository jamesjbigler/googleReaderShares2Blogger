using System;
using System.IO;

namespace Console
{
	public class ImportPosition
	{
		private const string FileName = "import_position.txt";
		public int BlogIndex { get; private set; }
		private DateTime LastImportTimestamp = DateTime.MinValue;
		public ImportPosition ()
		{
			if (File.Exists(FileName))
			{
				string[] lines = File.ReadAllLines(FileName);
				ParseBlogIndex(lines);
				ParseLastImportTimestamp(lines);
			}
		}

		private void ParseBlogIndex (string[] lines)
		{
			if (lines.Length > 0)
			{
				int index;
				bool parsed = Int32.TryParse(lines[0], out index);
				BlogIndex = (parsed) ? index : BlogIndex;
			}
		}

		private void ParseLastImportTimestamp (string[] lines)
		{
			if (lines.Length > 1)
			{
				DateTime date;
				bool parsed = DateTime.TryParse(lines[1], out date);
				LastImportTimestamp = (parsed) ? date : LastImportTimestamp;
			}
		}
		
		public static ImportPosition operator ++(ImportPosition pos) 
	   	{
	      	pos.BlogIndex++;
			pos.LastImportTimestamp = DateTime.Now;
			return pos;
	   	}
		
		public void Save()
		{
			var lines = new string[]
			{
				BlogIndex + "",
				LastImportTimestamp.ToShortDateString() + " " + LastImportTimestamp.ToShortTimeString()
			};
			File.WriteAllLines(FileName, lines);
		}
		
		public TimeSpan TimeToNextImport
		{
			get
			{
				DateTime endGoogleLockoutTime = LastImportTimestamp.AddDays(1).AddMinutes(1);
				if (LastImportTimestamp == DateTime.MinValue || DateTime.Now > endGoogleLockoutTime)
				{
					return new TimeSpan(0);	
				}
				return endGoogleLockoutTime - DateTime.Now;
			}
		}
	}
}

