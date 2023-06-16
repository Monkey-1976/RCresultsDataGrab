namespace GrabDRCCData;

public static class RaceData
{

    public static void CreateMeeting(RaceMeeting Meeting)
    {
        _meetings.Add(Meeting);
    }

    public static void AddRace(string MeetingName, Race Race)
    {
        RaceMeeting? meeting = _meetings.FirstOrDefault(m => m.Name == MeetingName);

        if (meeting != null)
        {
            meeting.Races.Add(Race);
        }
    }

    public static void AddRaceResult(string MeetingName, string RaceName, RaceResult Result)
    {
        RaceMeeting? meeting = _meetings.FirstOrDefault(m => m.Name == MeetingName);

        if (meeting != null)
        {
            Race? race = meeting.Races.FirstOrDefault(r => r.Name == RaceName);

            if(race != null)
            {
                race.RaceResults.Add(Result);
            }
        }
    }

    private static List<RaceMeeting> _meetings  = new List<RaceMeeting>();
}