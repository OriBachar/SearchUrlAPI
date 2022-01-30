using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net;
using HtmlAgilityPack;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SearchUrlAPI.SearchUrl;

namespace SearchUrlController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchUrlController : Controller
    {
        [HttpGet]
        public string Get(string url, int number)
        {

            List<string> list = new List<string>();

            List<SearchUrlAPI.SearchUrl.SearchUrl> data = new List<SearchUrlAPI.SearchUrl.SearchUrl>();

            list = GatAllUrls(url);

            if (list is null)
                return "The remote server returned an error";

            list.RemoveAll(match => !match.StartsWith("https://"));

            int minindex = number < list.Count() ? number : list.Count();

            for (int i = 0; i < minindex; i++)
            {
                Uri uri = new Uri(list[i]);

                string host = uri.Host;

               if (host.Contains("https://"))
                    host = host.Remove(host.IndexOf("https://"), "https://".Length);

                if (host.Contains("www."))
                    host = host.Remove(host.IndexOf("www."), "www.".Length);

                if (host.Contains(".co.il"))
                    host = host.Remove(host.IndexOf(".co.il"), ".co.il".Length);

                if (host.Contains(".com"))
                    host = host.Remove(host.IndexOf(".com"), ".com".Length);


                string title = host;

                data.Add(new SearchUrlAPI.SearchUrl.SearchUrl(list[i], title));
            }

            string json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);

            return json;
        }

        private List<string> GatAllUrls(string _url)
        {
            var client = new WebClient();

            try
            {
                var htmlSource = client.DownloadString(_url);

                var doc = new HtmlDocument();

                doc.LoadHtml(htmlSource);

                return doc.DocumentNode.SelectNodes("//a[@href]").Select(node => node.Attributes["href"].Value).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }

           

            

        }
    }
}
