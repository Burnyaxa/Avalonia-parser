using System;
using System.Collections.Generic;
using System.Linq;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using HtmlAgilityPack;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace AvaloniaParser.Parsers
{
    public class GenieDetailedParser : IParser
    {
        private const string InfoPath = "https://www.genie.co.kr/detail/songInfo?xgnm=";
        private const string ListXPath = "//*[@id=\"body-content\"]/div[6]/div/table/tbody";
        private const string AlbumXpath = "/ul/li[2]/span[2]/a";
        private const string ArtistXpath = "/ul/li[1]/span[2]/a";
        private const string DurationXpath = "/ul/li[4]/span[2]";
        private const string NameXpath = "/h2";
        private const int MaxParallelism = 4;
        public List<SongModel> Parse(string url)
        {
            List<SongModel> list = new List<SongModel>();
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() {"headless", "disable-gpu" });
            using var browser = new ChromeDriver(chromeOptions);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            browser.Navigate().GoToUrl(url);
            
            var results = browser.FindElementByXPath(ListXPath);
            var doc = new HtmlDocument();
            doc.LoadHtml(results.GetAttribute("innerHTML"));
            
            var nodes = doc.DocumentNode.SelectNodes("//tr[@class='list']");
            Parallel.ForEach(nodes,
                new ParallelOptions { MaxDegreeOfParallelism = MaxParallelism }, 
                node =>
            {
                using var browserInfo = new ChromeDriver(chromeOptions);
                string songId = node.GetAttributeValue("songid", "0");
                browserInfo.Navigate().GoToUrl(InfoPath + songId);
                HtmlDocument infoDoc = new HtmlDocument();
                results = browserInfo.FindElementByClassName("info-zone");
                
                infoDoc.LoadHtml(results.GetAttribute("innerHTML"));
                
                list.Add(new SongModel()
                {
                    Album = infoDoc.DocumentNode.SelectSingleNode(AlbumXpath).InnerText,
                    Artist = infoDoc.DocumentNode.SelectSingleNode(ArtistXpath).InnerText,
                    Duration = infoDoc.DocumentNode.SelectSingleNode(DurationXpath).InnerText,
                    Name = infoDoc.DocumentNode.SelectSingleNode(NameXpath).InnerText
                        .Replace(Environment.NewLine, string.Empty)
                        .Replace("\t", string.Empty)
                });
            });
            // foreach (var node in nodes)
            // {
            //     string songId = node.GetAttributeValue("songid", "0");
            //     browser.Navigate().GoToUrl(InfoPath + songId);
            //     HtmlDocument infoDoc = new HtmlDocument();
            //     results = browser.FindElementByClassName("info-zone");
            //     
            //     infoDoc.LoadHtml(results.GetAttribute("innerHTML"));
            //     
            //     list.Add(new SongModel()
            //     {
            // //         Album = infoDoc.DocumentNode.SelectSingleNode(AlbumXpath).InnerText,
            // Artist = infoDoc.DocumentNode.SelectSingleNode(ArtistXpath).InnerText,
            // Duration = infoDoc.DocumentNode.SelectSingleNode(DurationXpath).InnerText,
            // Name = infoDoc.DocumentNode.SelectSingleNode(NameXpath).InnerText
            //     .Replace(Environment.NewLine, string.Empty)
            //     .Replace("\t", string.Empty)
            //     });
            //}
            
            return list;
        }
    }
}