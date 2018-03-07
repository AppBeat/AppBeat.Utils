#region License
// Copyright (c) 2018 AppBeat Website Monitoring - https://www.appbeat.io
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;

namespace AppBeat.Utils.Email
{
    /// <summary>
    /// Provides offline implementation for determining if email is temporary / disposable.
    /// </summary>
    public partial class TemporaryEmailOfflineChecker : ITemporaryEmailChecker
    {
        /// <summary>
        /// Checks if email address or domain is categorized as temporary / disposable.
        /// </summary>
        /// <param name="emailOrDomain">Email address or email domain.</param>
        /// <returns>True if email address or domain is categorized as temporary / disposable, false otherwise.</returns>
        public bool IsTemporary(string emailOrDomain) => _normalizedDomains.Contains(GetNormalizedDomain(emailOrDomain));

        /// <summary>
        /// If email address is given, domain is extracted first and then normalized (invariant lower-case) domain is returned.
        /// </summary>
        /// <param name="emailOrDomain">Email address or email domain.</param>
        /// <returns>Normalized (invariant lower-case) domain.</returns>
        private static string GetNormalizedDomain(string emailOrDomain)
        {
            if (emailOrDomain == null)
            {
                throw new ArgumentNullException(nameof(emailOrDomain), "Missing email address or domain");
            }

            string domain;
            int atIndex = emailOrDomain.IndexOf('@');

            if (atIndex >= 0)
            {
                //email address was given, extract domain
                domain = emailOrDomain.Substring(atIndex + 1);
            }
            else
            {
                //domain was given
                domain = emailOrDomain;
            }

            domain = domain.Trim();
            if (Uri.CheckHostName(domain) == UriHostNameType.Unknown)
            {
                throw new ArgumentException("Invalid domain", nameof(emailOrDomain));
            }

            return domain.ToLowerInvariant();
        }

        /// <summary>
        /// If email domain is categorized as temporary you can whitelist it with this method. Domain is whitelisted as long as <seealso cref="TemporaryEmailOfflineChecker"/> instance is live.
        /// Method is thread-safe.
        /// </summary>
        /// <param name="emailOrDomain">Email address or email domain.</param>
        public void TryRemoveDomain(string emailOrDomain)
        {
            var normalizedDomain = GetNormalizedDomain(emailOrDomain);

            if (_normalizedDomains.Contains(normalizedDomain))
            {
                lock (_normalizedDomains)
                {
                    if (_normalizedDomains.Contains(normalizedDomain))
                    {
                        _normalizedDomains.Remove(normalizedDomain);
                    }
                }
            }
        }

        /// <summary>
        /// Add custom domain to list of temporary email domains. Domain is "blacklisted" as long as <seealso cref="TemporaryEmailOfflineChecker"/> instance is live.
        /// Method is thread-safe.
        /// </summary>
        /// <param name="emailOrDomain">Email address or email domain.</param>
        public void TryAddDomain(string emailOrDomain)
        {
            var normalizedDomain = GetNormalizedDomain(emailOrDomain);

            if (!_normalizedDomains.Contains(normalizedDomain))
            {
                lock (_normalizedDomains)
                {
                    if (!_normalizedDomains.Contains(normalizedDomain))
                    {
                        _normalizedDomains.Add(normalizedDomain);
                    }
                }
            }
        }

        /// <summary>
        /// Returns list of temporary domains for current instance.
        /// </summary>
        public IEnumerable<string> Domains
        {
            get
            {
                lock (_normalizedDomains)
                {
                    var lstDomains = new List<string>(_normalizedDomains.Count);
                    foreach (var domain in _normalizedDomains)
                    {
                        lstDomains.Add(domain);
                    }

                    return lstDomains.ToArray();
                }
            }
        }
    }
}
