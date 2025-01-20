using GitLabBlazor.Model;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GitLabBlazor.Model.DTOs;

namespace GitLabServicesLayer
{
	public interface IGitLabServices
	{
		Task<IEnumerable<GitLabProjects>> GetGitLabProjects(string privateToken,
			string gitLabAddress,
			CancellationToken cancellationToken=default);

		Task<List<ArtifactsDTO>> GetArtifacts(string privateToken, string gitLabAddress, int id,
			 CancellationToken cancellationToken);
	}
}
