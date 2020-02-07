using System.Collections.Generic;
using Leaf.xNet;
using UkrNetParse.Items;
using Newtonsoft.Json;
using System.Linq;
using System.IO;
using System;

namespace UkrNetParse.Parser
{
    public class Parser
    {

        private static HttpRequest request = new HttpRequest();
        private static string response;
        private static string GetSource(string url)
        {
            response = request.Get(url).ToString();
            
            return response;
        }

        private static SeoTitleItem[] GetSeoTitle(string source)
        {
            var seoTitleList = new List<SeoTitleItem>();
            try
            {
                NewsSeo result = JsonConvert.DeserializeObject<NewsSeo>(source);
                seoTitleList.AddRange(result.news.Where(n => n.seo != "false" && n.seo != ""));
                    
            }
            catch (System.Exception)
            {
  
            }
            
            return seoTitleList.ToArray();
        }

        private static TopsItem[] GetJsonSource(string source)
        {
            var newsList = new List<TopsItem>();
            News result = JsonConvert.DeserializeObject<News>(source);
            
            foreach (var item in result.tops.Where(i => DateTime.Now.Subtract(i.Time) <= TimeSpan.FromMinutes(60)))
            {
                if (item.News != null)
                {
                    newsList.AddRange(item.News);
                    continue;
                }
                newsList.Add(item);
                
            }


            return newsList.ToArray();
        }

        public static void Parsing()
        {
            List<TopsItem> allNews = new List<TopsItem>();
            var source = GetSource("https://www.ukr.net/ajax/news.json");
            var seo = GetSeoTitle(source);
            

            // foreach (var item in seo)
            // {
            //     source = GetSource($"https://www.ukr.net/news/dat/{item.seo}/1/");
            //     allNews.AddRange(GetJsonSource(source));
            //     System.Console.WriteLine(item.seo);
            // }

            source = GetSource($"https://www.ukr.net/news/dat/main/1/");
            allNews.AddRange(GetJsonSource(source));

            System.Console.WriteLine("Success parsing");

            AddToDb(allNews);


            // string resultJson = JsonConvert.SerializeObject(allNews);
            // File.AppendAllText("UkrJson.json", resultJson);
        }

        private static void AddToDb(List<TopsItem> newsList)
        {
            // List<DateBase.News> result = new List<DateBase.News>();
            // foreach(var item in newsList)
            // {
            //     result.Add(new DateBase.News()
            //     {
            //         Id = item.NewsId,
            //         Date = item.Time,
            //         Title = item.Title,
            //         Url = item.Url
            //     });
            // }

            System.Console.WriteLine("Begin add to db");
            // using (DateBase.testContext db = new DateBase.testContext())
            // {
            //     db.AddRange(result);
            // }
            DateBase.testContext db = new DateBase.testContext();
            //db.AddRange(result);
            
            foreach (var item in db.News)
            {
                System.Console.WriteLine(item.Id + " - " + item.Date);
            }
            System.Console.WriteLine("Success");
        }

    }
}