using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GrabDRCCData;

class Program
{
    static void Main(string[] args)
    {
        _logger.LogInformation("Starting...");

        // Dictionary<string, string> venues = WebScrapper.GetVenues();
        // List<Venue> venueList = new List<Venue>();

        // foreach (var venue in venues.OrderBy(v => v.Value))
        // {
        //     _logger.LogInformation($"id {venue.Key} - {venue.Value}");
        //     venueList.Add( new Venue
        //     {
        //         Id = Int32.Parse(venue.Key),
        //         Name = venue.Value
        //     });
        // }

        //_logger.LogInformation(JsonConvert.SerializeObject(venueList));

        List<RaceMeeting> meetings = WebScrapper.GetMeetings("https://www.rc-results.com/viewer/Main/VenueMeetings?venueId=21");

        List<OverallResult> overallResults = new List<OverallResult>();

        foreach (RaceMeeting meeting in meetings)
        {
            List<string> races = WebScrapper.GetRace("https://www.rc-results.com" + meeting.Url);
            List<RaceResult> raceResults = new List<RaceResult>();

            foreach (string race in races)
            {
                try
                {
 raceResults = WebScrapper.GetRaceResults("https://www.rc-results.com" + race);

                foreach (RaceResult result in raceResults)
                {

                    OverallResult? overallResult = meeting.OverallResults.FirstOrDefault(o => o.Driver == result.Driver);

                    string[] raceLaps = result.Result.Split('/');
                    TotalLaps totalLaps = new TotalLaps
                    {
                        Laps = Int32.TryParse(raceLaps[0].Trim(' '), out int numLaps) ? numLaps : 0,
                        TotalTime = float.TryParse(raceLaps[1].Trim(' '), out float time) ? time : 99999
                    };

                    if (overallResult == null)
                    {
                        overallResult = new OverallResult
                        {
                            Driver = result.Driver,
                            Class = meeting.Name,
                            BestLap = float.TryParse(result.Best, out float bestLap) ? bestLap : 99999,
                            Best10 = float.TryParse(result.Best10, out float best10) ? best10 : 99999,
                            BestLaps = totalLaps
                        };

                        meeting.OverallResults.Add(overallResult);
                    }
                    else
                    {
                        float newBest = float.TryParse(result.Best, out float bestLap) ? bestLap : 99999;

                        if ((newBest < overallResult.BestLap) && (newBest > 0))
                        {
                            overallResult.BestLap = newBest;
                        }

                        float newBest10 = float.TryParse(result.Best10, out float best10) ? best10 : 99999;

                        if((newBest10 < overallResult.Best10) && (newBest10 > 0))
                        {
                            overallResult.Best10 = newBest10;
                        }


                        if (totalLaps.Laps > overallResult.BestLaps.Laps)
                        {
                            overallResult.BestLaps = totalLaps;
                        }
                        else if ((overallResult.BestLaps.Laps == totalLaps.Laps) && (totalLaps.TotalTime < overallResult.BestLaps.TotalTime))
                        {
                            overallResult.BestLaps = totalLaps;
                        }
                    }
                }
                }
                catch
                {

                }

            }
        }



        foreach (RaceMeeting meeting in meetings)
        {
            _logger.LogInformation($"{meeting.Name} - {meeting.Date} - {meeting.Url}");

          OutputByFastestLap(meeting);
          OutputByLaps(meeting);
          OutputByDiffBestAverage(meeting);
        }

        _logger.LogInformation("Completed.");
    }

    private static void OutputByFastestLap(RaceMeeting meeting)
    {
        var file = new WriteFile(_filePath + meeting.Name.Replace(" ", "") + "_Fastest_" + meeting.Date.ToShortDateString().Replace("/", ""));

        int position = 1;

        float totalTime = 0;
        //foreach (OverallResult result in meeting.OverallResults.OrderByDescending(o => o.BestLaps.Laps).ThenBy(o => o.BestLaps.TotalTime))
        foreach (OverallResult result in meeting.OverallResults.OrderBy(o => o.BestLap))
        {
            if (result.BestLap > 0)
            {
                totalTime = totalTime + result.BestLap;
                _logger.LogInformation($"Position {position.ToString("00")} | {result.Driver},{result.BestLap},{result.Best10},{result.BestLaps.Laps},{result.BestLaps.TotalTime}");
                file.Write($"{result.Driver}: {result.BestLap}");
                position++;
            }
        }

        file.Write($"Average Best Lap {totalTime / position}");
    }

      private static void OutputByLaps(RaceMeeting meeting)
    {
        var file = new WriteFile(_filePath + meeting.Name.Replace(" ", "") + "_Laps_" + meeting.Date.ToShortDateString().Replace("/", ""));

        int position = 1;

        float totalLaps = 0;
        foreach (OverallResult result in meeting.OverallResults.OrderByDescending(o => o.BestLaps.Laps).ThenBy(o => o.BestLaps.TotalTime))
        {
            if (result.BestLap > 0)
            {
                totalLaps = totalLaps + result.BestLaps.Laps;
                _logger.LogInformation($"Position {position.ToString("00")} | {result.Driver},{result.BestLap},{result.Best10},{result.BestLaps.Laps},{result.BestLaps.TotalTime}");
                file.Write($"{result.Driver}: {result.BestLaps.Laps} - {result.BestLaps.TotalTime}");
                position++;
            }
        }

        file.Write($"Average Laps {totalLaps / position}");
    }

     private static void OutputByDiffBestAverage(RaceMeeting meeting)
    {
        var file = new WriteFile(_filePath + meeting.Name.Replace(" ", "") + "_BestAverage_" + meeting.Date.ToShortDateString().Replace("/", ""));

        int position = 1;
        foreach (OverallResult result in meeting.OverallResults.OrderBy(o => o.Best10 - o.BestLap))
        {
            if (result.BestLap > 0 && result.Best10 > 0)
            {
                _logger.LogInformation($"Position {position.ToString("00")} | {result.Driver},{result.BestLap},{result.Best10},{result.BestLaps.Laps},{result.BestLaps.TotalTime}");
                file.Write($"{result.Driver}: {result.Best10} - {result.BestLap} = {result.Best10 - result.BestLap}");
                position++;
            }
        }
    }


    private const string _filePath = "datafiles/";
    private static ILogger _logger = Log.Logger;
}