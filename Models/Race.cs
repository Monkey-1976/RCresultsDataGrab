namespace GrabDRCCData;

public class Race
{
    public string Name { get; set; } = string.Empty;
    public List<RaceResult> RaceResults { get; set; } = new List<RaceResult>();
    public DriverResult DriverResults { get; set; } = new DriverResult();
}