using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SearchUrlAPI.Models
{
    public class SearchUrl
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public SearchUrl(string url, string title)
        {
            Url = url;

            Title = title;
        }

    }
}
