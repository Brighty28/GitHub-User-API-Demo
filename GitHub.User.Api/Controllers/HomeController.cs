using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using GitHub.User.Api.Models;
using Microsoft.Ajax.Utilities;
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
            // https://github.com/octokit/octokit.net/blob/master/docs/getting-started.md
            var github = new GitHubClient(new ProductHeaderValue("GitHubAPIApp"));

            var request = new SearchUsersRequest(userName);

            var result = await github.Search.SearchUsers(request);

            if (result.TotalCount > 0 && result.Items[0].Login != null)
            {
                var user = await github.User.Get(userName);

                var repos = await github.Repository.GetAllForUser(userName);

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
            
            ModelState.AddModelError("No_users", "No items matched your search query!");

            return PartialView("_UsersList", new Models.User());

        }

    }
}
