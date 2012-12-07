#region License

/*
 * Copyright 2002-2012 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using CSharp.oDesk.Api.Impl;
using CSharp.oDesk.Api.Interfaces;
using Spring.Social.OAuth1;

namespace CSharp.oDesk.Connect
{
    /// <summary>
    /// oDesk <see cref="IServiceProvider"/> implementation.
    /// </summary>
    /// <author>Scott Smith</author>
    public class oDeskServiceProvider : AbstractOAuth1ServiceProvider<IoDesk>
    {
        /// <summary>
        /// Creates a new instance of <see cref="oDeskServiceProvider"/>.
        /// </summary>
        /// <param name="consumerKey">The application's API key.</param>
        /// <param name="consumerSecret">The application's API secret.</param>
        public oDeskServiceProvider(string consumerKey, string consumerSecret)
            : base(consumerKey, consumerSecret, new OAuth1Template(consumerKey, consumerSecret,
                "https://www.odesk.com/api/auth/v1/oauth/token/request",
                "https://www.odesk.com/services/api/auth",
                "https://www.odesk.com/api/auth/v1/oauth/token/access"))
        {
        }

        /// <summary>
        /// Returns an API interface allowing the client application to access protected resources on behalf of a user.
        /// </summary>
        /// <param name="accessToken">The API access token.</param>
        /// <param name="secret">The access token secret.</param>
        /// <returns>A binding to the service provider's API.</returns>
        public override IoDesk GetApi(string accessToken, string secret)
        {
            return new oDeskTemplate(ConsumerKey, ConsumerSecret, accessToken, secret);
        }
    }
}
