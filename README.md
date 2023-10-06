# GithubSearch
GithubSearch test application

1) Open solution and build

2) Run application via F5 button

3) Once application will be started open new browser tab and paste following line:
   http://localhost:33363/Home/SearchLatestOpenedPullRequestsJson?repositoryOwner=pavelsavara&repositoryName=runtime&pullRequestLabel=&customSearchKeywords=

Please pay your attention that some fields are empty. They empty in Github API response also

The result will look like below:

{
  "pullRequests": {
    "active": [
      {
        "pullRequestPageUrl": "https://github.com/pavelsavara/runtime/pull/7",
        "title": "[wasm] Bump chrome for testing - linux: 117.0.5938.132, windows: 117.0.5938.132",
        "description": null,
        "numberOfComments": 0,
        "creationDate": "2023-10-01T01:06:34Z",
        "closedDate": null,
        "creatorName": null,
        "creatorEmail": null,
        "avatarUrl": "https://avatars.githubusercontent.com/in/15368?v=4",
        "draft": false,
        "isStale": false,
        "countStaleDays": null,
        "countOfOpenedDays": 6,
        "commits": [
          {
            "commitHash": "4de0e071f8df4b6b2c01604515134c7ad717c658",
            "authorName": "github-actions[bot]",
            "authorEmail": "github-actions[bot]@users.noreply.github.com",
            "avatarUrl": "https://avatars.githubusercontent.com/in/15368?v=4",
            "message": "Automated bump of chrome version",
            "commitDate": "2023-10-01T01:06:32Z"
          }
        ]
      }
    ],
    "draft": [],
    "stale": [],
    "averageCountOfOpenedDaysForAllPullRequests": 6,
    "averageCountOfOpenedDaysForActivePullRequests": 6,
    "averageCountOfOpenedDaysForDraftPullRequests": 0,
    "averageCountOfOpenedDaysForStalePullRequests": 0
  }
}
