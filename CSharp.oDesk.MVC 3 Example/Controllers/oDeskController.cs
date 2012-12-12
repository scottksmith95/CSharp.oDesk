using System.Web.Mvc;
using CSharp.oDesk.Api.Interfaces;
using CSharp.oDesk.Connect;
using Spring.Json;
using Spring.Social.OAuth1;

namespace CSharp.oDesk.MVC_3_Example.Controllers
{
    public class oDeskController : Controller
    {
        // Configure the Callback URL
        private const string CallbackUrl = "http://localhost:14915/oDesk/Callback";

        // Set your consumer key & secret here
        private const string oDeskApiKey = "ENTER YOUR KEY HERE";
        private const string oDeskApiSecret = "ENTER YOUR SECRET HERE";

        readonly IOAuth1ServiceProvider<IoDesk> _oDeskProvider = new oDeskServiceProvider(oDeskApiKey, oDeskApiSecret);

        public ActionResult Index()
        {
            var token = Session["AccessToken"] as OAuthToken;
            if (token != null)
            {
                //Example showing how to make calls to API endpoints and auto using OAuth tokens
                var oDeskClient = _oDeskProvider.GetApi(token.Value, token.Secret);
                var result = oDeskClient.RestOperations.GetForObjectAsync<JsonValue>("https://www.odesk.com/api/auth/v1/info.json").Result;

                ViewBag.TokenValue = token.Value;
                ViewBag.TokenSecret = token.Secret;
                ViewBag.ResultText = result.ToString();

                return View();
            }

            var requestToken = _oDeskProvider.OAuthOperations.FetchRequestTokenAsync(CallbackUrl, null).Result;

            Session["RequestToken"] = requestToken;

            return Redirect(_oDeskProvider.OAuthOperations.BuildAuthenticateUrl(requestToken.Value, null));
        }

        public ActionResult Callback(string oauth_verifier)
        {
            var requestToken = Session["RequestToken"] as OAuthToken;
            var authorizedRequestToken = new AuthorizedRequestToken(requestToken, oauth_verifier);
            var token = _oDeskProvider.OAuthOperations.ExchangeForAccessTokenAsync(authorizedRequestToken, null).Result;

            Session["AccessToken"] = token;

            return RedirectToAction("Index");
        }
    }
}
