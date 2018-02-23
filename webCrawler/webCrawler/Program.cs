using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a URL: ");
            var url = Console.ReadLine();
            CrawlerManager manager = new CrawlerManager(url);
            
        }
    }
}
