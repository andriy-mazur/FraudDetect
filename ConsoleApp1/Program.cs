using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace Challenges
{
    // The task is to implement a simple concurrent & asynchronous web crawler.
    // Since we don't want to make it too complex, there is a test class in the
    // end that simulates page downloading by generating their content.
    // You can use it to test your solution.

    public class Page
    {
        public string Url { get; }
        public string Content { get; }

        public Page(string url, string content)
        {
            Url = url;
            Content = content;
        }

        public override string ToString() => $"Page, Url={Url}";
    }

    public interface ICrawler
    {
        // A delegate downloading the page by its URL
        Func<string, CancellationToken, Task<Page>> Downloader { get; set; }
        // A delegate extracting a sequence of references (~hrefs) found on the page
        Func<Page, IEnumerable<string>> ReferenceExtractor { get; set; }
        // Crawls the whole graph of pages completely.
        // Ideally, it should do this concurrently -- as efficiently as possible.
        Task<IDictionary<string, Page>>
            CrawlAsync(IEnumerable<string> urls, CancellationToken cancellationToken);
    }

    public class Crawler : ICrawler
    {
        public Func<string, CancellationToken, Task<Page>> Downloader { get; set; }
        public Func<Page, IEnumerable<string>> ReferenceExtractor { get; set; }

        private Dictionary<string, Page> result = new Dictionary<string, Page>();

        public async Task<IDictionary<string, Page>> CrawlAsync(IEnumerable<string> urls, CancellationToken cancellationToken)
        {
            foreach (var url in urls)
            {
                if (cancellationToken.IsCancellationRequested) return result;

                if (result.ContainsKey(url)) continue;
                result.Add(url, null);

                await GetPageAsync(url, cancellationToken);
            }

            return result;
        }

        private async Task GetPageAsync(string url, CancellationToken cancellationToken)
        {
            var page = await Downloader(url, cancellationToken);
            result[url] = page;

            var links = ReferenceExtractor(page);

            if (cancellationToken.IsCancellationRequested) return;
            await  CrawlAsync(links, cancellationToken);
        }
    }

    public class CrawlerTester
    {
        public ICrawler Crawler { get; set; }
        public int TotalPageCount { get; set; } = 10000;
        public TimeSpan MinPageDelayTime { get; set; } = TimeSpan.FromSeconds(0.1);
        public TimeSpan MaxPageDelayTime { get; set; } = TimeSpan.FromSeconds(1);
        public int MinPageReferenceCount { get; set; } = 0;
        public int MaxPageReferenceCount { get; set; } = 100;

        // Emulates reference extraction
        public IEnumerable<string> ExtractReferences(Page page) => page.Content.Split();

        // Emulates page downloading
        public async Task<Page> Download(string url, CancellationToken cancellationToken)
        {
            WriteLine($"+ {url}");
            // We want this method to behave the same every time it's called for the same URL
            var rnd = new Random(533000401 ^ url.GetHashCode());
            var delayDiff = MaxPageDelayTime - MinPageDelayTime;
            var delayTime = MinPageDelayTime +
                TimeSpan.FromSeconds(rnd.NextDouble() * delayDiff.TotalSeconds);
            var refCountDiff = MaxPageReferenceCount - MinPageReferenceCount;
            var refCount = MinPageReferenceCount + rnd.Next(refCountDiff);
            var content = string.Join(" ", Enumerable.Range(0, refCount).Select(_ => rnd.Next(TotalPageCount)));
            var page = new Page(url, content);
            await Task.Delay(delayTime, cancellationToken);
            WriteLine($"- {url} => {refCount} refs");
            return page;
        }

        // Emulates page downloading
        public async Task<Page> DownloadLongChain(string url, CancellationToken cancellationToken)
        {
            // We want this method to behave the same every time it's called for the same URL
            var rnd = new Random(533000401 ^ url.GetHashCode());
            var delayDiff = MaxPageDelayTime - MinPageDelayTime;
            var delayTime = MinPageDelayTime +
                TimeSpan.FromSeconds(rnd.NextDouble() * delayDiff.TotalSeconds);
            var nextPageIndex = int.Parse(url);
            if (nextPageIndex >= TotalPageCount)
                nextPageIndex = TotalPageCount;
            var content = $"{nextPageIndex}";
            var page = new Page(url, content);
            await Task.Delay(delayTime, cancellationToken);
            return page;
        }

        public void Test(params string[] startUrls)
        {
            Crawler.Downloader = Download;
            Crawler.ReferenceExtractor = ExtractReferences;
            WriteLine($"Start crawling, max. pages: {TotalPageCount}");

            var timer = new Stopwatch();
            timer.Start();
            var pages = Task.Run(() => Crawler.CrawlAsync(startUrls, CancellationToken.None)).Result;
            timer.Stop();

            var allRefs = new HashSet<string>(startUrls.Union(pages.Values.SelectMany(ExtractReferences)));
            WriteLine();
            WriteLine($"Time taken, seconds: {timer.Elapsed.TotalSeconds}");
            WriteLine($"Pages downloaded:    {pages.Count}");
            WriteLine($"All refs crawled?    {allRefs.Count == pages.Count}");
            if (allRefs.Count != pages.Count)
                throw new InvalidOperationException("Some refs aren't crawled.");
        }
    }

    public class SolutionC
    {
        // Rename to main before running
        public static void Main()
        {
            var tester = new CrawlerTester();
            var crawler = new Crawler()
            {
                Downloader = tester.Download,
                ReferenceExtractor = tester.ExtractReferences
            };
            tester.Crawler = crawler;
            tester.Test("0");
        }
    }
}