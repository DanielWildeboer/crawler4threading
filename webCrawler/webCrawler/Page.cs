using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCrawler
{
    public class Page
    {
        private Uri _url;
        private List<HtmlNode> _inputList;
        private List<Exception> _exceptionList;

        public Page(Uri url)
        {
            _url = url;
            _inputList = new List<HtmlNode>();
            _exceptionList = new List<Exception>();
        }

        public Uri url
        {
            get { return _url; }
            set { _url = value; }
        }

        public List<HtmlNode> inputList
        {
            get { return _inputList; }
            set
            {
                _inputList = value;
            }
        }

        public List<Exception> exceptionList
        {
            get { return _exceptionList; }
            set
            {
                _exceptionList = value;
            }
        }
    }
}
