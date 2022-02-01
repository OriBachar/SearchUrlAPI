﻿using Microsoft.AspNetCore.Mvc;
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

            list = GetAllUrls(url);

            if (list is null)
                return "The remote server returned an error";

            list.RemoveAll(match => !match.StartsWith("https://"));

            int minindex = number < list.Count() ? number : list.Count();

            for (int i = 0; i < minindex; i++)
            {
                Uri uri = new Uri(list[i]);

                if (uri is null)
                    return "Null Pointer Exception";

                string host = uri.Host;

                if (host.Contains("www."))
                    host = host.Replace("www.","");

                if (host.Contains(".co.il"))
                    host = host.Replace(".co.il", "");

                if (host.Contains(".com"))
                    host = host.Replace(".com", "");


                string title = host;

                data.Add(new SearchUrlAPI.SearchUrl.SearchUrl(list[i], title));
            }

            string json = JsonConvert.SerializeObject(data.ToArray(), Formatting.Indented);

            return json;
        }

        private List<string> GetAllUrls(string _url)
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