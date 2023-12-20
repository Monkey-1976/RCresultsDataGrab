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


    private static List<RaceMeeting> _meetings  = new List<RaceMeeting>();
}