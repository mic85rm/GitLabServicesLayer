using System;
using System.Collections.Generic;

namespace GitLabBlazor.Model
{
	public class GitLabJobs
	{
		public Commit commit { get; set; }
		//public object coverage { get; set; }
		//public bool archived { get; set; }
		//public bool allow_failure { get; set; }
		public DateTime? created_at { get; set; }
		//public DateTime started_at { get; set; }
		//public DateTime finished_at { get; set; }
		//public object erased_at { get; set; }
		//public double duration { get; set; }
		//public double queued_duration { get; set; }
		//public ArtifactsFile? artifacts_file { get; set; }
		public List<Artifact> artifacts { get; set; }
		//public DateTime artifacts_expire_at { get; set; }
		//public List<string> tag_list { get; set; }
		public int? id { get; set; }
		//public string name { get; set; }
		public Pipeline pipeline { get; set; }
		//public string @ref { get; set; }
		//public Runner runner { get; set; }
		//public RunnerManager runner_manager { get; set; }
		//public string stage { get; set; }
		//public string status { get; set; }
		//public string failure_reason { get; set; }
		//public bool tag { get; set; }
		//public string web_url { get; set; }
		//public Project project { get; set; }
		//public User user { get; set; }
	}
}
