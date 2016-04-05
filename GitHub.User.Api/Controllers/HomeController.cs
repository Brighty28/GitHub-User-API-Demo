using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GitHub.User.Api.Models;
using Octokit;

namespace GitHub.User.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        //
        // GET: /SearchUsers/

        [HttpPost]
        public async Task<ActionResult> SearchUsers(string userName)
        {
            var github = new GitHubClient(new ProductHeaderValue("GitHubAPIApp"));

            var user = await github.User.Get(userName);

            // https://github.com/octokit/octokit.net/blob/master/docs/getting-started.md
            //var user = "https://api.github.com/users" + "/" + user_name;

            var repos = await github.Repository.GetAllForUser(userName); //.Get(userName, userName);

            var reposFiltered = repos.OrderByDescending(r => r.StargazersCount).Take(5);

            var userDetails = new Models.User()
            {
                AvatarUrl = user.AvatarUrl,
                Location = user.Location,
                Name = userName != null ? user.Name : userName,
                RepoList = reposFiltered.ToList()
            };
            return PartialView("_UsersList", userDetails);
        }

    }
}
