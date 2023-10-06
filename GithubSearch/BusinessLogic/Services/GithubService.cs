using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Services
{
    public class GithubService: IGithubService
    {
        private readonly IGithubRepository _githubRepository;
        private readonly IOptions<GithubConfig> _githubConfig;

        public GithubService(IGithubRepository githubRepository, IOptions<GithubConfig> githubConfig)
        {
            _githubRepository = githubRepository;
            _githubConfig = githubConfig;
        }

        public PullRequestResponseViewModel SearchLatestOpenedPullRequests(string repositoryOwner, string repositoryName, string pullRequestLabel, string customSearchKeywords)
        {
            var pullRequestsUrl = string.Format(_githubConfig.Value.GithubPullRequestsApiUrl, repositoryOwner, repositoryName);
            var pullRequests = _githubRepository.Get<List<PullRequestModel>>(pullRequestsUrl);
            if(pullRequests == null)
            {
                return new PullRequestResponseViewModel();
            }

            var filteredPullRequests = pullRequests.Where(pr => FilterPullRequest(pr, pullRequestLabel, customSearchKeywords));

            var pullRequestsList = new List<PullRequestViewModel>();
            foreach (var pullRequest in filteredPullRequests)
            {
                var commits = GetPullRequestCommits(pullRequest);

                var pullRequestUserData = _githubRepository.Get<PullRequestUserDataModel>(pullRequest.User.Url);
                
                var comments = _githubRepository.Get<List<PullRequestCommentsModel>>(pullRequest.CommentsUrl);

                var pullRequestViewModel = GetPullRequestViewModel(pullRequest, comments, commits, pullRequestUserData);
                pullRequestsList.Add(pullRequestViewModel);

            }

            return new PullRequestResponseViewModel
            {
                PullRequests = GetSearchPullRequestResponseViewModel(pullRequestsList)
            };
        }

        private bool FilterPullRequest(PullRequestModel pullRequest, string pullRequestLabel, string customSearchKeywords)
        {
            return (string.IsNullOrEmpty(pullRequestLabel) || pullRequest.Labels.Any(l => l.Name == pullRequestLabel)) &&
                (string.IsNullOrEmpty(customSearchKeywords) || pullRequest.Title.Contains(customSearchKeywords));
        }

        private SearchPullRequestResponseViewModel GetSearchPullRequestResponseViewModel(List<PullRequestViewModel> pullRequestsList)
        {
            var activePullRequests = pullRequestsList.Where(pr => !pr.IsStale && !pr.Draft);
            var draftPullRequests = pullRequestsList.Where(pr => pr.Draft);
            var stalePullRequests = pullRequestsList.Where(pr => pr.IsStale);

            int averageCountOfOpenedDaysForAllGroups = pullRequestsList.Count() > 0? Convert.ToInt32(pullRequestsList.Average(pr => pr.CountOfOpenedDays)): 0;
            int averageCountOfOpenedDaysForActiveGroups = activePullRequests.Count() > 0 ? Convert.ToInt32(activePullRequests.Average(pr => pr.CountOfOpenedDays)): 0;
            int averageCountOfOpenedDaysForDraftGroups = draftPullRequests.Count() > 0 ? Convert.ToInt32(draftPullRequests.Average(pr => pr.CountOfOpenedDays)): 0;
            int averageCountOfOpenedDaysForStaleGroups = stalePullRequests.Count() > 0 ? Convert.ToInt32(stalePullRequests.Average(pr => pr.CountOfOpenedDays)): 0;

            return new SearchPullRequestResponseViewModel
            {
                Active = activePullRequests,
                Draft = draftPullRequests,
                Stale = stalePullRequests,
                AverageCountOfOpenedDaysForAllPullRequests = averageCountOfOpenedDaysForAllGroups,
                AverageCountOfOpenedDaysForActivePullRequests = averageCountOfOpenedDaysForActiveGroups,
                AverageCountOfOpenedDaysForDraftPullRequests = averageCountOfOpenedDaysForDraftGroups,
                AverageCountOfOpenedDaysForStalePullRequests = averageCountOfOpenedDaysForStaleGroups,
            };
        }

        private PullRequestViewModel GetPullRequestViewModel(PullRequestModel pullRequest,
            List<PullRequestCommentsModel> comments,
            IEnumerable<PullRequestCommitViewModel> commits,
            PullRequestUserDataModel pullRequestUserData)
        {
            bool isStale = !pullRequest.Draft && pullRequest.CreationDate.AddMonths(1) < DateTime.Now;

            int? countOfStaleDays = isStale ? Convert.ToInt32((DateTime.Now - pullRequest.CreationDate.AddMonths(1)).TotalDays) : null;

            int countOfOpenedDays = pullRequest.ClosedDate == null?
                 Convert.ToInt32((DateTime.Now - pullRequest.CreationDate).TotalDays)
                : Convert.ToInt32((pullRequest.ClosedDate - pullRequest.CreationDate)?.TotalDays);

            return new PullRequestViewModel
            {
                PullRequestPageUrl = pullRequest.PullRequestPageUrl,
                Title = pullRequest.Title,
                Description = pullRequest.Description,
                NumberOfComments = comments?.Count ?? 0,
                CreationDate = pullRequest.CreationDate,
                ClosedDate = pullRequest.ClosedDate,
                CreatorName = pullRequestUserData.Name,
                CreatorEmail = pullRequestUserData.Email,
                AvatarUrl = pullRequest.User?.AvatarUrl,
                Draft = pullRequest.Draft,
                IsStale = isStale,
                CountStaleDays = countOfStaleDays,
                CountOfOpenedDays = countOfOpenedDays,
                Commits = commits,
            };
        }

        private IEnumerable<PullRequestCommitViewModel> GetPullRequestCommits(PullRequestModel pullRequest)
        {
            var commits = _githubRepository.Get<List<PullRequestCommitModel>>(pullRequest.CommitsUrl);

            return commits == null ? 
                new List<PullRequestCommitViewModel>() :
                commits.Select(c => new PullRequestCommitViewModel 
                {
                    CommitHash = c.CommitHash,
                    AuthorName = c.Commit?.Author?.Name,
                    AuthorEmail = c.Commit?.Author?.Email,
                    AvatarUrl = c.Author.AvatarUrl,
                    CommitDate = c.Commit?.Author?.Date,
                    Message = c.Commit?.Message
                });
        }

    }
}
