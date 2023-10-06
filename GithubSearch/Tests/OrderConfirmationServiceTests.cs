using System;
using System.Linq;
using Tests.Contexts;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using BusinessLogic.Models;

namespace Tests
{
    class OrderConfirmationServiceTests : GithubServiceTestsContext
    {
        [Test]
        public void SearchLatestOpenedPullRequests_ReturnPullRequests_ReturnPullRequestsData()
        {
            // Arrange
            var githubService = GetGithubService();

            // Act
            var result = githubService.SearchLatestOpenedPullRequests("repositoryOwner", "repositoryName", null, null );

            // Assert          
            Assert.AreEqual(result.PullRequests.Active.Count(), 2);
            Assert.AreEqual(result.PullRequests.Draft.Count(), 1);
            Assert.AreEqual(result.PullRequests.Stale.Count(), 1);

            var stalePullRequest = result.PullRequests.Stale.FirstOrDefault();
            Assert.AreEqual(stalePullRequest.CountStaleDays, Convert.ToInt32((DateTime.Now - stalePullRequest.CreationDate.AddMonths(1)).TotalDays));

            var activePullRequest = result.PullRequests.Active.FirstOrDefault();
            Assert.AreEqual(activePullRequest.CountOfOpenedDays, Convert.ToInt32((activePullRequest.ClosedDate - activePullRequest.CreationDate)?.TotalDays));

            var allPullRequestsList = result.PullRequests.Active.Concat(result.PullRequests.Draft).Concat(result.PullRequests.Stale);
            int averageCountOfOpenedDaysForAllGroups = Convert.ToInt32(allPullRequestsList.Average(pr => pr.CountOfOpenedDays));
            Assert.AreEqual(result.PullRequests.AverageCountOfOpenedDaysForAllPullRequests, averageCountOfOpenedDaysForAllGroups);

            int averageCountOfOpenedDaysForActiveGroup = Convert.ToInt32(result.PullRequests.Active.Average(pr => pr.CountOfOpenedDays));
            Assert.AreEqual(result.PullRequests.AverageCountOfOpenedDaysForActivePullRequests, averageCountOfOpenedDaysForActiveGroup);

            int averageCountOfOpenedDaysForDraftGroup = Convert.ToInt32(result.PullRequests.Draft.Average(pr => pr.CountOfOpenedDays));
            Assert.AreEqual(result.PullRequests.AverageCountOfOpenedDaysForDraftPullRequests, averageCountOfOpenedDaysForDraftGroup);

            int averageCountOfOpenedDaysForStaleGroup = Convert.ToInt32(result.PullRequests.Stale.Average(pr => pr.CountOfOpenedDays));
            Assert.AreEqual(result.PullRequests.AverageCountOfOpenedDaysForStalePullRequests, averageCountOfOpenedDaysForStaleGroup);
        }

        [Test]
        public void SearchLatestOpenedPullRequests_NoPullRequests_ReturnEmptyPullRequests()
        {
            // Arrange
            var githubRepository = GetGithubRepository(false);
            var githubService = GetGithubService(githubRepository);

            // Act
            var result = githubService.SearchLatestOpenedPullRequests("repositoryOwner", "repositoryName", null, null);

            // Assert          
            Assert.AreEqual(result.PullRequests, null);
        }

        [Test]
        public void SearchLatestOpenedPullRequests_FilteredByLabelRequests_ReturnPullRequestsWithLabel()
        {
            // Arrange
            var githubService = GetGithubService();

            // Act
            var result = githubService.SearchLatestOpenedPullRequests("repositoryOwner", "repositoryName", "test", null );

            // Assert          
            Assert.AreEqual(result.PullRequests.Active.Count(), 1);
            Assert.AreEqual(result.PullRequests.Draft.Count(), 0);
            Assert.AreEqual(result.PullRequests.Stale.Count(), 0);
        }

        [Test]
        public void SearchLatestOpenedPullRequests_FilteredBycustomSearchKeywords_ReturnPullRequestsWithCustomSearchKeywords()
        {
            // Arrange
            var githubService = GetGithubService();

            // Act
            var result = githubService.SearchLatestOpenedPullRequests("repositoryOwner", "repositoryName", null, "test 4");

            // Assert          
            Assert.AreEqual(result.PullRequests.Active.Count(), 0);
            Assert.AreEqual(result.PullRequests.Draft.Count(), 0);
            Assert.AreEqual(result.PullRequests.Stale.Count(), 1);
        }
    }
}
