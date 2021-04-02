using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

namespace AvaloniaParser.Parsers
{
    public class GenieParser : IParser
    {
        //private const string ListXPath = "//*[@id=\"body-content\"]/div[6]/div/table/tbody";
        private const string ListXPath = "//tbody";
        private const string NameXPath = "td[5]/a[1]";
        private const string ArtistXPath = "td[5]/a[2]";
        private const string AlbumXPath = "td[5]/a[3]";
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
                node =>
                { 
                    list.Add(new SongModel()
                    {
                        Name = node.SelectSingleNode(NameXPath).InnerText
                            .Replace(Environment.NewLine, string.Empty)
                            .Replace("\t", string.Empty)
                            .Replace("&amp;", "&")
                            .Trim(),
                        Album = node.SelectSingleNode(AlbumXPath).InnerText
                            .Replace(Environment.NewLine, string.Empty)
                            .Replace("\t", string.Empty)
                            .Replace("&amp;", "&"),
                        Artist = node.SelectSingleNode(ArtistXPath).InnerText
                            .Replace(Environment.NewLine, string.Empty)
                            .Replace("\t", string.Empty)
                            .Replace("&amp;", "&"),
                        Duration = string.Empty
                    });
                });
            
            return list;
        }
    }
}