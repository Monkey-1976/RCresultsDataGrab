namespace GrabDRCCData;

public class RaceResult
{
    public int Position { get; set;  }
    public int Car { get; set; }
    public string Result { get; set; } = string.Empty;
    public float Average { get; set; }
    public float Best10 { get; set; }
    public float Best { get; set; }
    public float ConSec3 { get; set; }
    public int Laps { get; set; }

}