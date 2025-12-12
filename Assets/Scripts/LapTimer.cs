using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    public List<Collider> sectorTriggers = new List<Collider>();
    [SerializeField] private string FileName = "LapTimes.json";

    private float lapStartTime;
    private float sectorStartTime;
    private int nextSectorIndex = -1;

    private Dictionary<int, float> sectorTimes = new Dictionary<int, float>();

    #region Unity
    void Start()
    {
        for (int i = 0; i < sectorTriggers.Count; i++)
        {
            SectorTrigger trigger = sectorTriggers[i].gameObject.AddComponent<SectorTrigger>();
            trigger.parentTimer = this;
            trigger.myIndex = i;
        }
    }

    private void OnSectorTriggered(int index)
    {
        if (nextSectorIndex == -1)
        {
            lapStartTime = Time.time;
            sectorStartTime = Time.time;

            nextSectorIndex = 1;
            return;
        }

        if (index != nextSectorIndex)
            return;

        float now = Time.time;
        float sectorTime = Round3(now - sectorStartTime);

        sectorTimes[index] = sectorTime;

        Debug.Log($"Sector {index} time: {sectorTime}s");

        sectorStartTime = now;

        if (index == 0 && nextSectorIndex == 0 && now != lapStartTime)
        {
            float lapTime = Round3(now - lapStartTime);
            Debug.Log($"LAP COMPLETED: {lapTime}s");

            List<float> sectorsCopy = new List<float>(sectorTimes.Values);
            SaveLapTime(lapTime, sectorsCopy);

            lapStartTime = Time.time;
            sectorStartTime = Time.time;

            nextSectorIndex = 1;
            return;
        }

        nextSectorIndex = (nextSectorIndex + 1) % sectorTriggers.Count;
    }

    public class SectorTrigger : MonoBehaviour
    {
        public LapTimer parentTimer;
        public int myIndex;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                parentTimer.OnSectorTriggered(myIndex);
            }
        }
    }
    #endregion

    #region Saving
    public void SaveLapTime(float lapTime, List<float> sectors)
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);
        Debug.Log("Full file path: " + path);

        TimeStats data;

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
            Debug.Log($"New best lap time: {lapTime} (last best: {data.FastestTime}");
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
                    Debug.Log($"New best time in sector {i}: {sectors[i]} (last best: {data.SectorTimes[i]}");
                    data.SectorTimes[i] = sectors[i];
                }
            }
        }

        string newJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, newJson);

        Debug.Log("Saved lap time: " + lapTime);
    }

    float Round3(float v)
    {
        return Mathf.Round(v * 1000f) / 1000f;
    }

    public TimeStats LoadAll()
    {
        string path = Path.Combine(Application.persistentDataPath, FileName);

        if (!File.Exists(path))
            return new TimeStats();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<TimeStats>(json);
    }

    public List<float> LoadLapTimes()
    {
        return LoadAll().LapTimes;
    }

    public List<float> LoadSectorTimes()
    {
        return LoadAll().SectorTimes;
    }

    public float LoadFastestTime()
    {
        return LoadAll().FastestTime;
    }
}

[System.Serializable]
public class TimeStats
{
    public float FastestTime;
    public List<float> SectorTimes = new List<float>();
    public List<float> LapTimes = new List<float>();
}

#endregion