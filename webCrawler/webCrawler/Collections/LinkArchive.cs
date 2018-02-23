using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webCrawler.Collections
{
    public class LinkArchive
    {
        private Queue<Uri> _validUrlQueue;

        private List<Uri> _invalidUrl;
        private List<Uri> _externalUrl;
        private Dictionary<Uri, Page> _visitedUrl;

        public LinkArchive()
        {
            _validUrlQueue = new Queue<Uri>();
            _invalidUrl = new List<Uri>();
            _externalUrl = new List<Uri>();
            _visitedUrl = new Dictionary<Uri, Page>();
        }

        public Queue<Uri> validUrlQueue
        {
            get { return _validUrlQueue; }
            set { _validUrlQueue = value; }
        }
        public int queueSize
        {
            get { return _validUrlQueue.Count; }
        }
        public List<Uri> invalidUrl
        {
            get { return _invalidUrl; }
            set { _invalidUrl = value; }
        }
        public List<Uri> externalUrl
        {
            get { return _externalUrl; }
            set { _externalUrl = value; }
        }
        public Dictionary<Uri, Page> visitedUrl
        {
            get { return _visitedUrl; }
            set { _visitedUrl = value; }
        }
    }
}
