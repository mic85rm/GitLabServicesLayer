﻿namespace GitLabBlazor.Model
{
	public class GitLabNameSpace
	{
		public int id { get; set; }
		public string name { get; set; }
		public string path { get; set; }
		public string kind { get; set; }
		public string full_path { get; set; }
		public object parent_id { get; set; }
		public object avatar_url { get; set; }
		public string web_url { get; set; }
	}
}
