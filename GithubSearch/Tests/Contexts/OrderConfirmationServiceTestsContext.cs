using System;
using System.Collections.Generic;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace Tests.Contexts
{
    internal class GithubServiceTestsContext
    {
        private const string GithubPullRequestsApiUrlConst = "https://api.github.com/repos/{0}/{1}/pulls?state=open";

        protected IGithubService GetGithubService(IGithubRepository githubRepository = null, IOptions<GithubConfig> githubConfig = null)
        {
            var orderConfirmationService = new GithubService(githubRepository ?? GetGithubRepository(), githubConfig ?? GetGithubConfig());

            return orderConfirmationService;
        }

        protected IGithubRepository GetGithubRepository(bool shouldReturnPullRequestsData = true)
        {
            var mock = Mock.Of<IGithubRepository>();

            if(shouldReturnPullRequestsData)
            {
                Mock.Get(mock).Setup(m => m.Get<List<PullRequestModel>>(It.IsAny<string>())).Returns(GetPullRequestModels());
                Mock.Get(mock).Setup(m => m.Get<List<PullRequestCommitModel>>(It.IsAny<string>())).Returns(GetPullRequestCommitModels());
                Mock.Get(mock).Setup(m => m.Get<PullRequestUserDataModel>(It.IsAny<string>())).Returns(GetPullRequestUserDataModel());
                Mock.Get(mock).Setup(m => m.Get<List<PullRequestCommentsModel>>(It.IsAny<string>())).Returns(GetPullRequestCommentsModels());
            }

            return mock;
        }

        protected IOptions<GithubConfig> GetGithubConfig()
        {
            var mock = Mock.Of<IOptions<GithubConfig>>();

            Mock.Get(mock).Setup(m => m.Value).Returns(Mock.Of<GithubConfig>(gc=>gc.GithubPullRequestsApiUrl == GithubPullRequestsApiUrlConst));

            return mock;
        }

        private List<PullRequestCommentsModel> GetPullRequestCommentsModels()
        {
            return new List<PullRequestCommentsModel> 
            {
                new PullRequestCommentsModel(),
                new PullRequestCommentsModel()
            };
        }

        private PullRequestUserDataModel GetPullRequestUserDataModel()
        {
            return new PullRequestUserDataModel
            {
                Name = "Test User",
                Email = "test@gmail.com"
            };
        }

        private List<PullRequestCommitModel> GetPullRequestCommitModels()
        {
            return new List<PullRequestCommitModel>
            {
                new PullRequestCommitModel
                {
                    CommitHash = "testtest",
                    Author = new PullRequestCommitAuthorModel(),
                    Commit = new PullRequestCommitDataModel
                    {
                        Author = new PullRequestCommitAuthorDataModel(),
                    }
                }
            };
        }

        private List<PullRequestModel> GetPullRequestModels()
        {
            return new List<PullRequestModel>
            {
                new PullRequestModel //Draft
                {
                    User = new PullRequestUserModel(),
                    Title = "test 1",
                    Draft = true,
                    Description = "Draft pull request",
                },
                new PullRequestModel //Active
                {
                    User = new PullRequestUserModel(),
                    Title = "test 2",
                    Description = "Active pull request",                    
                    CreationDate = DateTime.Now.AddDays(-25),
                    ClosedDate = DateTime.Now.AddDays(-5),
                    Labels = new List<PullRequestLabelModel>{ new PullRequestLabelModel { Name = "test"} }
                },
                new PullRequestModel //Active
                {
                    User = new PullRequestUserModel(),
                    Title = "test 3",
                    Description = "Active pull request",
                    CreationDate = DateTime.Now.AddDays(-10)                    
                },
                new PullRequestModel //Stale
                {
                    User = new PullRequestUserModel(),
                    Title = "test 4",
                    Description = "Stale pull request",
                    CreationDate = DateTime.Now.AddDays(-32)
                }
            };
        }

    }
}
