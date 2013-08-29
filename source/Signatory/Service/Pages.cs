using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Tavis;

namespace Signatory.Service
{
    public class Pages
    {
        public Pages(HttpResponseHeaders headers)
        {
            if (headers.Contains("Link"))
            {
                var links = headers.ParseLinkHeaders(new Uri("https://api.github.com/"), new LinkRegistry());

                var nextLink = links.SingleOrDefault(link => link.Relation == "next");
                var firstLink = links.SingleOrDefault(link => link.Relation == "first");
                var lastLink = links.SingleOrDefault(link => link.Relation == "last");
                var previousLink = links.SingleOrDefault(link => link.Relation == "prev");

                if (nextLink != null) Next = Convert.ToInt32(nextLink.Target.ParseQueryString()["page"]);
                if (firstLink != null) First = Convert.ToInt32(firstLink.Target.ParseQueryString()["page"]);
                if (lastLink != null) Last = Convert.ToInt32(lastLink.Target.ParseQueryString()["page"]);
                if (previousLink != null) Previous = Convert.ToInt32(previousLink.Target.ParseQueryString()["page"]);
            }
        }

        public int? First { get; set; }
        public int? Last { get; set; }
        public int? Next { get; set; }
        public int? Previous { get; set; }
    }
}