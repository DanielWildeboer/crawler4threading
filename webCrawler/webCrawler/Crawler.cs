using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using webCrawler;

public class Crawler
{
    
    public Uri baseURL;
    public Page page;
    private CrawlerManager manager;

    public Crawler(Uri url, CrawlerManager manager)
    {
        this.manager = manager;
        baseURL = url;
        page = new Page(url);
    }

    internal void Crawl()
    {
        var rootDoc = getHTML(baseURL);
        if (rootDoc == null) return;
        
        try
        {
            page.inputList = fetchAllInputs(rootDoc);
            seperatePageUrls(fetchAllHrefs(rootDoc));            
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception thrown: {0}", e.Message);
            page.exceptionList.Add(e);
        }
       
        finished();
    }

    private HtmlDocument getHTML(Uri Url)
    {
        HtmlDocument doc = new HtmlDocument();
        HtmlWeb website = new HtmlWeb();

        try
        {
            HtmlDocument rootDocument = website.Load(Url.ToString());
            return rootDocument;
        }
        catch (Exception e)
        {
            page.exceptionList.Add(e);
            CrawlerManager.archive.invalidUrl.Add(Url);
            return null;
        }       
    }

    private void finished()
    {
        manager.crawlerFinished(baseURL, page, this);
    }

    private void seperatePageUrls(List<Uri> list)
    {
        //Console.WriteLine("LINK LIST SIZE: {0}", list.Count);
        foreach (var item in list)
        {
            Uri outUri;
               
            if (!Uri.TryCreate(item.ToString(), UriKind.Absolute, out outUri) || !(outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps) || item.ToString().Contains(" ") || page.exceptionList.Count > 0)
            {
                //Console.WriteLine("Invalid link added: {0}", item);
                CrawlerManager.archive.invalidUrl.Add(item);
            }
            else if (item.Host != baseURL.Host)
            {
                //Console.WriteLine("External link added {0}" , item);
                CrawlerManager.archive.externalUrl.Add(item);
            }            
            else if(!CrawlerManager.archive.validUrlQueue.Contains(item))
            {
                //Console.WriteLine("Added: {0} to valid links", item);
                CrawlerManager.archive.validUrlQueue.Enqueue(item);
            }
        }
    }

    public List<Uri> fetchAllHrefs(HtmlDocument file)
    {
        List<Uri> list = new List<Uri>();

        HtmlDocument htmlDoc = file;

        foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
        {
            list.Add(new Uri(baseURL, link.GetAttributeValue("href", String.Empty)));            
        }

        //Console.WriteLine("LinkList size: {0}", list.Count);
        return list;
    }

    public List<HtmlNode> fetchAllInputs(HtmlDocument file)
    {
        List<HtmlNode> inputNode = new List<HtmlNode>();

        HtmlDocument htmlDoc = file;

        try
        {
            foreach (HtmlNode input in htmlDoc.DocumentNode.SelectNodes("//input | //textarea"))
            {
                inputNode.Add(input);
            }

        }
        catch (Exception e)
        {
            page.exceptionList.Add(e);
        }
        //Console.WriteLine("InputNodeList size: {0}", inputNode.Count);
        return inputNode;
    }

}
