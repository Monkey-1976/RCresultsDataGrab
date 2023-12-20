namespace GrabDRCCData;

public class OverallResult
{
    public string Driver { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public float BestLap { get; set; }
    public float Best10 { get; set; }
    public TotalLaps BestLaps{ get; set; } = new TotalLaps();
    public float BestConsec3 { get; set; }
    public string FinalBand { get; set; } = string.Empty;
    public int QualifyPostion { get; set; }
    public int FinalPosition { get; set; }
}