using Microsoft.Extensions.Logging;

using HtmlAgilityPack;

namespace GrabDRCCData;

public static class WebScrapper
{
    public static List<RaceMeeting> GetMeetings(string url)
    {
        List<RaceMeeting> raceMeetings = new List<RaceMeeting>();

        var web = new HtmlWeb();
        var doc = web.Load(url);

        // var doc = new HtmlDocument();
        // doc.Load("html.html");

        HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//tbody");


        foreach (var item in bodyNode.ChildNodes)
        {

            if(item.HasChildNodes)
            {
                string[] splits = item.InnerText.Split("\r\n");
                string href = string.Empty;

                foreach (var child in item.ChildNodes)
                {
                    if(child.HasChildNodes)
                    {
                     foreach (var aref in child.ChildNodes)
                     {
                        HtmlAttribute att = aref.Attributes["href"];
                        href = att.Value;
                     }
                    }

                }

                RaceMeeting raceMeeting = new RaceMeeting
                {
                    Name = splits[2].Trim(' '),
                    Date = DateTime.Parse(splits[1].Trim(' ')),
                    Url = href

                };

                raceMeetings.Add(raceMeeting);
            }
        }

        return raceMeetings;
    }

    /// <summary>
    /// Go through race meeting page and find URL's with raceid in them
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static void GetRace(string url)
    {
        // var web = new HtmlWeb();
        // var doc = web.Load(url);

        var doc = new HtmlDocument();
        doc.Load("htmlmeeting.html");

        foreach (HtmlNode item in doc.DocumentNode.SelectNodes("//a[@href]"))
        {

            if(item.OuterHtml.Contains("raceId"))
            {

                _logger.LogInformation($"{item.Attributes["href"].Value}");
            }
             string[] splits = item.InnerText.Split("\r\n");
                string href = string.Empty;

                foreach (var child in item.ChildNodes)
                {
                    if(child.HasChildNodes)
                    {
                     foreach (var aref in child.ChildNodes)
                     {
                        HtmlAttribute att = aref.Attributes["href"];
                        href = att.Value;
                        _logger.LogInformation($"{href}");
                     }
                    }

                }
        }
    }

    public static void GetRaceResults()
    {

        var doc = new HtmlDocument();
        doc.Load("htmlRaceResults.html");



        HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//tbody");


        foreach (HtmlNode row in bodyNode.SelectNodes("tr"))
        {
            string[] results = new string[6];
            int pos = 0;
            foreach (HtmlNode col in row.SelectNodes("td"))
            {
                _logger.LogInformation($"{col.InnerText}");
                results[pos] = col.InnerText;
                pos++;
            }
            
        }

    }

    public static void GetDriverTimes()
    {

    }

    private static readonly string _baseUrl = "https://www.rc-results.com";

    private static ILogger _logger = Log.Logger;
}