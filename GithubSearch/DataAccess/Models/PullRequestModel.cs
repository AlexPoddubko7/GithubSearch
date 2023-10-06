using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataAccess.Models
{
    public class PullRequestModel
    {
        [JsonProperty("html_url")]
        public string PullRequestPageUrl { get; set; }

        public string Title { get; set; }

        [JsonProperty("body")]
        public string Description { get; set; }

        public string State { get; set; }

        public bool Locked { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("closed_at")]
        public DateTime? ClosedDate { get; set; }

        [JsonProperty("commits_url")]
        public string CommitsUrl { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }

        public bool Draft { get; set; }

        public PullRequestUserModel User { get; set; }

        public List<PullRequestLabelModel> Labels { get; set; } = new List<PullRequestLabelModel>();
    }
}
