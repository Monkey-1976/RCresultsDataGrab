namespace GrabDRCCData;

public class DriverResult
{
    public int Position { get; set;  }
    public string Result { get; set; } = string.Empty;
    public float Average { get; set; }
    public float Best10 { get; set; }
    public float Best { get; set; }
    public float ConSec3 { get; set; }
    public List<LapTime> Laps { get; set; }
}