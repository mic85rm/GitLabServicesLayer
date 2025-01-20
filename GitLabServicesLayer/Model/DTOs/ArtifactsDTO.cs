using System;

namespace GitLabBlazor.Model.DTOs
{
	public class ArtifactsDTO
	{
		public int id { get; set; }
		public DateTime created_at { get; set; }
		public string file_type { get; set; }
		public int size { get; set; }
		public string filename { get; set; }
		public string file_format { get; set; }

		public int idPipeline { get; set; }
		public string idCommit { get; set; }
		public string short_id { get; set; }
		
	}
}
