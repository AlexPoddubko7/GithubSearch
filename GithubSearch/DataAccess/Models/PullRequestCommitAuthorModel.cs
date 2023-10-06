using Newtonsoft.Json;

namespace DataAccess.Models
{
    public class PullRequestCommitAuthorModel
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
    }
}
