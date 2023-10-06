using Newtonsoft.Json;

namespace DataAccess.Models
{
    public class PullRequestCommitModel
    {
        [JsonProperty("sha")]
        public string CommitHash { get; set; }

        public PullRequestCommitAuthorModel Author { get; set; }

        public PullRequestCommitDataModel Commit { get; set; }

    }
}
