using System;
using System.Collections.Generic;

namespace BusinessLogic.Models
{
    public class PullRequestViewModel
    {
        public string PullRequestPageUrl { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int NumberOfComments { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        public string CreatorName { get; set; }

        public string CreatorEmail { get; set; }

        public string AvatarUrl { get; set; }

        public bool Draft { get; set; }

        public bool IsStale { get; set; }

        public int? CountStaleDays { get; set; }

        public int CountOfOpenedDays { get; set; }

        public IEnumerable<PullRequestCommitViewModel> Commits { get; set; }
    }
}
