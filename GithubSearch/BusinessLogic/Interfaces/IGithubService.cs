using BusinessLogic.Models;

namespace BusinessLogic.Interfaces
{
    public interface IGithubService
    {
        PullRequestResponseViewModel SearchLatestOpenedPullRequests(string repositoryOwner, string repositoryName, string pullRequestLabel, string customSearchKeywords);
    }
}
