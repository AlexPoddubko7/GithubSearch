namespace DataAccess.Models
{
    public class PullRequestCommitDataModel
    {
        public PullRequestCommitAuthorDataModel Author { get; set; }

        public string Message { get; set; }
    }
}
