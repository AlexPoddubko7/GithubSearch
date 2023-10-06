using System;

namespace BusinessLogic.Models
{
    public class PullRequestCommitViewModel
    {
        public string CommitHash { get; set; }
        
        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        public string AvatarUrl { get; set; }

        public string Message { get; set; }

        public DateTime? CommitDate { get; set; }
    }
}
