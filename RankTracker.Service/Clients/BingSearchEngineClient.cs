using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using RankTracker.Service.Configuration;
using RankTracker.Service.Factories;

namespace RankTracker.Service.Clients
{
    public class BingHeadlessSearchEngineClient : BaseHeadlessSearchEngineClient, ISearchEngineClient
    {
        public BingHeadlessSearchEngineClient(
            IBrowserContextFactory contextFactory,
            IOptions<SearchEngineOptions> options)
            : base(contextFactory, options)
        {
            base.selector = "#b_content";
        }


        protected override string BuildSearchUrl(string query)
            => $"https://www.bing.com/search" +
               $"?q={Uri.EscapeDataString(query)}" +
               $"&count={Options.DefaultResultCount}";

        protected override List<(string Domain, int Position)> ExtractDomainPositions(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var resultsList = doc.GetElementbyId("b_results");
            if (resultsList == null)
                return new List<(string, int)>();

            var items = resultsList
                .Descendants("li")
                .Where(li => li.Attributes["class"] != null)
                .ToList();

            var results = new List<(string Domain, int Position)>(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                var li = items[i];
                var link = li
                    .Descendants("h2")
                    .SelectMany(h2 => h2.Descendants("a"))
                    .FirstOrDefault(a => a.Attributes["href"] != null);

                if (link == null)
                    continue;

                var href = link.GetAttributeValue("href", "").Trim();
                if (!Uri.IsWellFormedUriString(href, UriKind.Absolute))
                    continue;

                var uri = new Uri(href);
                var domain = uri.Host;

                results.Add((Domain: domain, Position: i + 1));
            }

            return results;
        }
    }
}
