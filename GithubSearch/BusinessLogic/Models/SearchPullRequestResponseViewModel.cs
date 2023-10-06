using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class SearchPullRequestResponseViewModel
    {
        public IEnumerable<PullRequestViewModel> Active { get; set; }

        public IEnumerable<PullRequestViewModel> Draft { get; set; }

        public IEnumerable<PullRequestViewModel> Stale { get; set; }

        public int AverageCountOfOpenedDaysForAllPullRequests { get; set; }

        public int AverageCountOfOpenedDaysForActivePullRequests { get; set; }

        public int AverageCountOfOpenedDaysForDraftPullRequests { get; set; }

        public int AverageCountOfOpenedDaysForStalePullRequests { get; set; }
    }
}
