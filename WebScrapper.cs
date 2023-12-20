using Microsoft.Extensions.Logging;

using HtmlAgilityPack;

namespace GrabDRCCData;

public static class WebScrapper
{
    public static Dictionary<string, string> GetVenues()
    {
        Dictionary<string, string> venues = new Dictionary<string, string>();

        var web = new HtmlWeb();
        var doc = web.Load("https://www.rc-results.com/viewer/");

        foreach (HtmlNode item in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            if (item.OuterHtml.Contains("venueId"))
            {
                string[] urlSplit = item.Attributes["href"].Value.Split("=", 2);

                venues.Add(urlSplit[1], item.InnerHtml);
            }
        }

        return venues;
    }
    public static List<RaceMeeting> GetMeetings(string url)
    {
        List<RaceMeeting> raceMeetings = new List<RaceMeeting>();

        var web = new HtmlWeb();
        var doc = web.Load(url);

        #region Testing
        // var doc = new HtmlDocument();
        // doc.Load("html.html");
        #endregion

        HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//tbody");


        foreach (var item in bodyNode.ChildNodes)
        {

            if (item.HasChildNodes)
            {
                string[] splits = item.InnerText.Split("\r\n");
                string href = string.Empty;

                foreach (var child in item.ChildNodes)
                {
                    if (child.HasChildNodes)
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
    public static List<string> GetRace(string url)
    {
        List<string> races = new List<string>();
        var web = new HtmlWeb();
        var doc = web.Load(url);

        // var doc = new HtmlDocument();
        // doc.Load("htmlmeeting.html");

        foreach (HtmlNode item in doc.DocumentNode.SelectNodes("//a[@href]"))
        {
            if (item.OuterHtml.Contains("raceId"))
            {
                races.Add(item.Attributes["href"].Value);
            }
        }
        return races;
    }

    public static List<RaceResult> GetRaceResults(string url)
    {
        List<RaceResult> raceResults = new List<RaceResult>();

        var web = new HtmlWeb();
        var doc = web.Load(url);


        // var doc = new HtmlDocument();
        // doc.Load("htmlRaceResults.html");

        HtmlAgilityPack.HtmlNode bodyNode = doc.DocumentNode.SelectSingleNode("//tbody");

        foreach (HtmlNode row in bodyNode.SelectNodes("tr"))
        {
            string[] results = new string[6];
            int pos = 0;

            foreach (HtmlNode col in row.SelectNodes("td"))
            {
                results[pos] = col.InnerText;
                pos++;
            }

            RaceResult result = new RaceResult
            {
                Position = results[0],
                Car = results[1],
                Driver = results[2],
                Result = results[3],
                Best10 = results[4],
                Best = results[5]
            };

            raceResults.Add(result);

        }

        return raceResults;

    }

    public static void GetDriverTimes()
    {

    }

    private static readonly string _baseUrl = "https://www.rc-results.com";

    private static ILogger _logger = Log.Logger;
}