using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCrawler
{
    public struct LinkItem
    {
        public Uri Href;

        public override string ToString()
        {
            return Href.ToString();
        }
    }
}
