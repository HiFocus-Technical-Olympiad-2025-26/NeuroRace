using System.Collections;
using System.Collections.Generic;

public static class LiveLapState
{
    public static float CurrentLapTime;
    public static List<float> CurrentSectors = new List<float>();
    public static int CurrentSectorIndex;

    public static float PreviousLapTime;
    public static List<float> PreviousSectors = new List<float>();
}