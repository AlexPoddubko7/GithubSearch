using BusinessLogic.Interfaces;
using GithubSearch.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GithubSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGithubService _githubService;

        public HomeController(ILogger<HomeController> logger, 
            IGithubService githubService)
        {
            _logger = logger;
            _githubService = githubService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchLatestOpenedPullRequests(string repositoryOwner, string repositoryName,string pullRequestLabel = null, string customSearchKeywords = null)
        {
            var searchPullRequestResponseViewModel = _githubService.SearchLatestOpenedPullRequests(repositoryOwner, repositoryName, pullRequestLabel, customSearchKeywords);
            return View(searchPullRequestResponseViewModel);
        }

        public JsonResult SearchLatestOpenedPullRequestsJson(string repositoryOwner, string repositoryName, string pullRequestLabel = null, string customSearchKeywords = null)
        {
            var searchPullRequestResponseViewModel = _githubService.SearchLatestOpenedPullRequests(repositoryOwner, repositoryName, pullRequestLabel, customSearchKeywords);
            return Json(searchPullRequestResponseViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
