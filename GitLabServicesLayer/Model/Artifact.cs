namespace GitLabBlazor.Model
{
	public class Artifact
	{
		public string file_type { get; set; } = "";
		public int size { get; set; } = 0;
		public string filename { get; set; } = "";
		public string file_format { get; set; } = "";
	}
}
