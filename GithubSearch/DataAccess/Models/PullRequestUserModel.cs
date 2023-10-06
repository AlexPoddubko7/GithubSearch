using Newtonsoft.Json;

namespace DataAccess.Models
{
    public class PullRequestUserModel
    {
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }

        public string Url { get; set; }// get name and email from this url
    }
}
