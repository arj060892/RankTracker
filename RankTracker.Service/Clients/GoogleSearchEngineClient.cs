using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using RankTracker.Service.Configuration;
using RankTracker.Service.Factories;

namespace RankTracker.Service.Clients
{
    public class GoogleHeadlessSearchEngineClient : BaseHeadlessSearchEngineClient
    {
        public GoogleHeadlessSearchEngineClient(
            IBrowserContextFactory contextFactory,
            IOptions<SearchEngineOptions> options)
            : base(contextFactory, options)
        {
            base.selector = "#res";
        }

        protected override string BuildSearchUrl(string query)
            => $"https://www.google.co.uk/search" +
               $"?num={Options.DefaultResultCount}" +
               $"&q={Uri.EscapeDataString(query)}";

        protected override List<(string Domain, int Position)> ExtractDomainPositions(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var resDiv = doc.GetElementbyId("res");
            if (resDiv == null)
                return new List<(string, int)>();

            var resultNodes = resDiv
                .Descendants("div")
                .Where(n => n.GetClasses().Contains("MjjYud"))
                .ToList();

            var results = new List<(string Domain, int Position)>(resultNodes.Count);

            for (int i = 0; i < resultNodes.Count; i++)
            {
                var node = resultNodes[i];
                var anchor = node.Descendants("a")
                                 .FirstOrDefault(a => a.Attributes["href"] != null);
                if (anchor == null)
                    continue;

                var href = anchor.GetAttributeValue("href", "").Trim();
                if (!Uri.IsWellFormedUriString(href, UriKind.Absolute))
                    continue;

                results.Add((Domain: href, Position: i + 1));
            }

            return results;
        }
    }

    internal static class HtmlNodeExtensions
    {
        public static IEnumerable<string> GetClasses(this HtmlNode node)
        {
            var cls = node.GetAttributeValue("class", "");
            return cls.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}