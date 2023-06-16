namespace GrabDRCCData;

public class RaceMeeting
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Url { get; set; } = string.Empty;
    public List<Race> Races { get; set; } = new List<Race>();

}