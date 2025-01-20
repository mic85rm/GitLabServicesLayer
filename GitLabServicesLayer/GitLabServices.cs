using GitLabBlazor.Model;
using GitLabBlazor.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GitLabServicesLayer
{
	public class GitLabServices : IGitLabServices
	{
		private readonly IHttpClientFactory _httpClient;

		public GitLabServices(IHttpClientFactory httpClient)
		{
			_httpClient = httpClient;
		}


		public async Task<IEnumerable<GitLabProjects>> GetGitLabProjects(string privateToken, string gitLabAddress, CancellationToken cancellationToken = default)
		{
			var http = _httpClient.CreateClient();
			http.DefaultRequestHeaders.Add("PRIVATE-TOKEN",privateToken);
			

			IEnumerable<GitLabProjects> projects = Enumerable.Empty<GitLabProjects>();
			HttpResponseMessage res = await http.GetAsync($"{gitLabAddress}api/v4/projects", cancellationToken).ConfigureAwait(false);
			if (res.IsSuccessStatusCode)
			{
				int numberOfTotalProjects = Convert.ToInt32(res.Headers.GetValues("X-Total")?.FirstOrDefault() ?? "0");
				res = await http.GetAsync($"{gitLabAddress}/api/v4/projects?pagination=keyset&per_page=100&order_by=id&sort=asc&simple=true", cancellationToken).ConfigureAwait(false);
				var intermediateProjects = await res.Content.ReadFromJsonAsync<IEnumerable<GitLabProjects>>(cancellationToken).ConfigureAwait(false);
				projects = projects.Concat(intermediateProjects);
				if (numberOfTotalProjects > 100)
				{
					for (int i = 0; i < numberOfTotalProjects; i += 100)
					{
						cancellationToken.ThrowIfCancellationRequested();
						string linkNext = res.Headers.GetValues("Link")?.FirstOrDefault() ?? "";
						string url = linkNext.Split(';')[0].Trim('<', '>');
						res = await http.GetAsync(url, cancellationToken).ConfigureAwait(false);
						intermediateProjects = await res.Content.ReadFromJsonAsync<IEnumerable<GitLabProjects>>(cancellationToken).ConfigureAwait(false);
						projects = projects.Concat(intermediateProjects);
					}
				}
			}
			return projects;

		}



		public async Task<List<ArtifactsDTO>> GetArtifacts(string privateToken, string gitLabAddress, int id, CancellationToken cancellationToken)
		{
			//using (HttpClient http = new HttpClient(new AddHeadersDelegatingHandler(privateToken)))
			//{

			var http = _httpClient.CreateClient();
			http.DefaultRequestHeaders.Add("PRIVATE-TOKEN", privateToken);
			IEnumerable<GitLabJobs> gitLabJobs = Enumerable.Empty<GitLabJobs>();
			List<ArtifactsDTO> artifacts = new List<ArtifactsDTO>();
			string pattern = @"<(https://[^>]+)>; rel=""next""";
			string nextJobs = $"{gitLabAddress}api/v4/projects/{id}/jobs?page=1&per_page=100";

			try
			{
				HttpResponseMessage res = await http.GetAsync(nextJobs, cancellationToken).ConfigureAwait(false);
				if (res.IsSuccessStatusCode)
				{
					do
					{
						
						res = await http.GetAsync(nextJobs,cancellationToken).ConfigureAwait(false);
						cancellationToken.ThrowIfCancellationRequested();
						nextJobs = res.Headers.GetValues("Link")?.FirstOrDefault() ?? "";
						var intermediateJobs = await res.Content
							.ReadFromJsonAsync<IEnumerable<GitLabJobs>>(cancellationToken).ConfigureAwait(false);
						intermediateJobs = intermediateJobs?.Where(z => z.artifacts.Any()).AsEnumerable();
						gitLabJobs = gitLabJobs.Concat(intermediateJobs ?? Enumerable.Empty<GitLabJobs>());

						if (!string.IsNullOrEmpty(nextJobs))
						{
							Match match = Regex.Match(nextJobs, pattern);
							if (match.Success && match.Groups.Count == 2)
							{
								nextJobs = match.Groups[1].Value;
							}
							else
							{
								nextJobs = "";
							}
						}
					} while (!string.IsNullOrEmpty(nextJobs));
				}

				if (gitLabJobs.Any())
				{
					artifacts = gitLabJobs
						.SelectMany(labJobs => labJobs?.artifacts?.Select(artifact => new ArtifactsDTO
						{
							created_at = labJobs.created_at ?? DateTime.MinValue,
							file_type = artifact.file_type,
							size = artifact.size,
							filename = artifact.filename,
							file_format = artifact.file_format,
							id = labJobs.id ?? 0,
							idCommit = labJobs?.commit?.id ?? "",
							short_id = labJobs?.commit?.short_id ?? "",
							idPipeline = labJobs?.pipeline?.id ?? -1
						}))
						.ToList();
				}
			}
			catch (OperationCanceledException)
			{
			}
			//catch (TaskCanceledException e) when (cancellationToken.IsCancellationRequested)
			//{
			//	Debug.WriteLine("Request was canceled: " + e.Message);

			//}
			//catch (TaskCanceledException e)
			//{
			//	Debug.WriteLine("Task was canceled: " + e.Message);
			//}
			catch (Exception e)
			{
				Debug.WriteLine("An error occurred: " + e.Message);
			}

			return artifacts;
		}
	}



}
