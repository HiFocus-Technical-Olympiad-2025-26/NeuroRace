using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class TimeStats
{
    public float FastestTime;
    public List<float> SectorTimes = new List<float>();
    public List<float> LapTimes = new List<float>();
}


public static class LapTimesSaver
{
    public static event System.Action OnLapSaved;

    private static string GetPath(string FileName) => Path.Combine(Application.persistentDataPath, FileName);

    public static TimeStats LoadAll(string fileName)
    {
        string path = GetPath(fileName);

        if (!File.Exists(path))
            return new TimeStats();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TimeStats>(json);
    }

    public static List<float> LoadLapTimes(string fileName) => LoadAll(fileName).LapTimes;

    public static List<float> LoadSectorTimes(string fileName) => LoadAll(fileName).SectorTimes;

    public static float LoadFastestTime(string fileName) => LoadAll(fileName).FastestTime;

    public static void SaveLapTime(float lapTime, List<float> sectors, string fileName)
    {
        TimeStats data;

        string path = GetPath(fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<TimeStats>(json);
        }
        else
        {
            data = new TimeStats();
        }

        data.LapTimes.Add(lapTime);

        if (data.FastestTime == 0f || lapTime < data.FastestTime)
        {
            //Debug.Log($"New best lap time: {lapTime} (last best: {data.FastestTime}");
            data.FastestTime = lapTime;
        }

        if (data.SectorTimes == null || data.SectorTimes.Count == 0)
        {
            data.SectorTimes = new List<float>(sectors);
        }
        else
        {
            for (int i = 0; i < sectors.Count; i++)
            {
                if (sectors[i] < data.SectorTimes[i])
                {
                    //Debug.Log($"New best time in sector {i}: {sectors[i]} (last best: {data.SectorTimes[i]}");
                    data.SectorTimes[i] = sectors[i];
                }
            }
        }

        File.WriteAllText(path, JsonUtility.ToJson(data, true));

        //Debug.Log("Saved lap time: " + lapTime);

        OnLapSaved?.Invoke();
    }
}
