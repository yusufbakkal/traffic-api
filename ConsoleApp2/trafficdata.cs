using System.Collections.Generic;

public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class Coordinates
{
    public List<Coordinate> Coordinate { get; set; }
}

public class FlowSegmentData
{
    public string Frc { get; set; }
    public int CurrentSpeed { get; set; }
    public int FreeFlowSpeed { get; set; }
    public int CurrentTravelTime { get; set; }
    public int FreeFlowTravelTime { get; set; }
    public double Confidence { get; set; }
    public bool RoadClosure { get; set; }
    public Coordinates Coordinates { get; set; } 
}

public class TrafficData
{
    public FlowSegmentData FlowSegmentData { get; set; }
    
}
