using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using DynamicData;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

namespace AvaloniaParser.Parsers
{
    public class GenieParser : IParser
    {
        private const string ListXPath = "//tbody";
        private const string NameXPath = "td/a[@class=\"title ellipsis\"]";
        private const string ArtistXPath = "td/a[@class=\"artist ellipsis\"]";
        private const string AlbumXPath = "td/a[@class=\"albumtitle ellipsis\"]";
        public List<SongModel> Parse(string url)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() {"headless", "disable-gpu" });
            using var browser = new ChromeDriver(chromeOptions);
            browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            browser.Navigate().GoToUrl(url);
            
            var results = browser.FindElementByXPath(ListXPath);
            var doc = new HtmlDocument();
            doc.LoadHtml(results.GetAttribute("innerHTML"));
            
            var hoverListNode = doc.DocumentNode.SelectSingleNode("//tr[@class='list hover']");
            var nodes = doc.DocumentNode.SelectNodes("//tr[@class='list']");
            
            if (hoverListNode != null)
            {
                nodes.Insert(0, hoverListNode);
            }

            return (from node in nodes let nameNode = node.SelectSingleNode(NameXPath)
                let name = ConvertNode(nameNode)
                let albumNode = node.SelectSingleNode(AlbumXPath)
                let album = ConvertNode(albumNode)
                let artistNode = node.SelectSingleNode(ArtistXPath)
                let artist = ConvertNode(artistNode)
                select new SongModel()
                {
                    Name = name,
                    Album = album,
                    Artist = artist,
                    Duration = string.Empty
                }).ToList();
        }

        private string ConvertNode(HtmlNode node)
        {
            return node is null ? string.Empty : node.InnerText
                .Replace(Environment.NewLine, string.Empty)
                .Replace("\t", string.Empty)
                .Replace("&amp;", "&")
                .Trim();
        }
    }
}