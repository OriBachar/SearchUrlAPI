using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchUrlAPI.SearchUrl
{
    public class SearchUrl
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public SearchUrl(string url, string title)
        {
            this.Url = url;

            this.Title = title;
        }

        public SearchUrl()
        {
        }
    }
}
