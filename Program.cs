using Microsoft.Extensions.Logging;

namespace GrabDRCCData;

class Program
{
    static void Main(string[] args)
    {
        _logger.LogInformation("Starting...");

        // List<RaceMeeting> meetings = WebScrapper.GetMeetings("https://www.rc-results.com/viewer/Main/VenueMeetings?venueId=21");

        // foreach (RaceMeeting race in meetings)
        // {
        //     _logger.LogInformation($"{race.Name} - {race.Date} - {race.Url}");
        // }

        WebScrapper.GetRaceResults();

        _logger.LogInformation("Completed.");
    }

    private static ILogger _logger = Log.Logger;
}