using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Octokit;

namespace GitHub.User.Api.Models
{
    public class User
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string AvatarUrl { get; set; }

        public List<Repository> RepoList { get; set; }

    }

    public class RepoListItem
    {
        public string RepoName { get; set; }

        public string Url { get; set; }

        public int? StargazersCount { get; set; }
    }
}