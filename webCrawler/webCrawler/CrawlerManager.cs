using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using webCrawler.Collections;

namespace webCrawler
{
    public class CrawlerManager
    {
        private Uri baseUrl;
        private static volatile List<Crawler> crawlerList;
        internal static LinkArchive archive;
        private bool running;
        private static readonly object threadLock = new object();

        public CrawlerManager(string baseUrl)
        {
            archive = new LinkArchive();
            crawlerList = new List<Crawler>();
            running = true;
            try
            {
                this.baseUrl = new Uri(baseUrl);
            }
            catch (UriFormatException)
            {
                Console.WriteLine("Invalid URL, exiting program...");
                Console.Read();
            }    
            catch (ArgumentNullException)
            {
                Console.WriteLine("URL is null, exiting program...");
                Console.Read();
            }
            archive.validUrlQueue.Enqueue(this.baseUrl);
            managerKeepAlive();
        }

        private void managerKeepAlive()
        {
            while (running)
            {
                if (crawlerList.Count < 7 && archive.queueSize > 0)
                {
                    Task.Run(() =>
                    {
                        startCrawler();
                    });                    
                }


                Thread.Sleep(10);
                if (archive.queueSize == 0 && crawlerList.Count == 0)
                {
                    Console.WriteLine("Crawlers finished, Page count: {0}", archive.visitedUrl.Count);
                    parseHtmlNodes();                    
                }
                //Console.WriteLine("Crawlers running now: {0}", crawlerList.Count);
            }
        }

        private void startCrawler()
        {
            Uri url;
            if (archive.queueSize > 0)
            {
                lock (threadLock)
                {
                    url = archive.validUrlQueue.Dequeue();
                    if (archive.visitedUrl.ContainsKey(url)) return;                                      
                }
            }
            else
            {
                Console.WriteLine("No more links in queue, exiting...");
                
                return;
            }
            Crawler crawler = new Crawler(url, this);
            //Console.WriteLine("=========================================================================================");
            Console.WriteLine("Starting crawler with URL: " + url);
            crawlerList.Add(crawler);
            archive.visitedUrl.Add(url, null);
            //Console.WriteLine("Starting crawler");
            
            crawler.Crawl();
            
        }

        internal void crawlerFinished(Uri url, Page page, Crawler crawler)
        {
            //Console.WriteLine("Crawlers running: {0}", crawlerList.Count);
            if (archive.visitedUrl.ContainsKey(url))
            {
                archive.visitedUrl[url] = page;
                //Console.WriteLine("Crawler updated archive with visited URL: " + archive.visitedUrl[url].url);
            }
            else
            {
                Console.WriteLine("No URL found with: " + url);
            }
            if (crawlerList.Contains(crawler))
            {
                crawlerList.Remove(crawler);
            }
            else
            {
                Console.WriteLine("WARNING: NO CRAWLER FOUND IN THE LIST WHILE REMOVING!");
            }            
        }

        private void parseHtmlNodes()
        {
            Console.WriteLine("======== Started parsing nodes ========");
            foreach (var item in archive.visitedUrl)
            {
                Console.WriteLine("Visited URL: {0} ", item.Value.url);
                
                foreach (var inputNode in item.Value.inputList)
                {
                    try
                    {
                        if (inputNode.Attributes["type"].Value != "hidden")
                        {
                            Console.WriteLine("inputNode name: {0}", inputNode.Attributes["name"].Value);
                        }                        
                    }
                    catch (Exception)
                    {
                        //catch errors
                    } 
                }
                Console.WriteLine("====================================================");
            }

            Console.WriteLine("Statistics: {0} external links, {1} invalid links and {2} valid links", archive.externalUrl.Count, archive.invalidUrl.Count, archive.visitedUrl.Count);
            Console.Read();
        }
    }
}
