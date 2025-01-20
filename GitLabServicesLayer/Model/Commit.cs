using System;

namespace GitLabBlazor.Model
{

	public class Commit
	{
		public string author_email { get; set; }
		public string author_name { get; set; }
		public DateTime created_at { get; set; }
		public string id { get; set; }
		public string message { get; set; }
		public string short_id { get; set; }
		public string title { get; set; }
	}
}
