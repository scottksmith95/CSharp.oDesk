using System;
using System.Diagnostics;
using CSharp.oDesk.Api;
using CSharp.oDesk.Connect;
using Spring.Json;
using Spring.Social.OAuth1;

namespace CSharp.oDesk.Console_Example
{
    class Program
    {
        // Set your consumer key & secret here
        private const string oDeskApiKey = "ENTER YOUR KEY HERE";
        private const string oDeskApiSecret = "ENTER YOUR SECRET HERE";

        private static void Main(string[] args)
        {
            try
            {
                var oDeskServiceProvider = new oDeskServiceProvider(oDeskApiKey, oDeskApiSecret);

                /* OAuth 'dance' */

                // Authentication using Out-of-band/PIN Code Authentication
                Console.Write("Getting request token...");
                var oauthToken = oDeskServiceProvider.OAuthOperations.FetchRequestTokenAsync("oob", null).Result;
                Console.WriteLine("Done");

                var authenticateUrl = oDeskServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
                Console.WriteLine("Redirect user for authentication: " + authenticateUrl);
                Process.Start(authenticateUrl);
                Console.WriteLine("Enter PIN Code from oDesk authorization page:");
                var pinCode = Console.ReadLine();

                Console.Write("Getting access token...");
                var requestToken = new AuthorizedRequestToken(oauthToken, pinCode);
                var oauthAccessToken = oDeskServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
                Console.WriteLine("Done");

                /* API */

                var oDesk = oDeskServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

                oDesk.RestOperations.GetForObjectAsync<JsonValue>("https://www.odesk.com/api/auth/v1/info.json")
                    .ContinueWith(task => Console.WriteLine("Result: " + task.Result.ToString()));
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex is oDeskApiException)
                    {
                        Console.WriteLine(ex.Message);
                        return true;
                    }
                    return false;
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("--- hit <return> to quit ---");
                Console.ReadLine();
            }
        }
    }
}