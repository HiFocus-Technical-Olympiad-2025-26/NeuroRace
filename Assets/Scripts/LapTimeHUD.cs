using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LapTimeHUD : MonoBehaviour
{
    [Header("Rows")]
    [SerializeField] private Transform currentRow;
    [SerializeField] private Transform bestRow;
    [SerializeField] private Transform previousRow;

    private TextMeshProUGUI curTotal;
    private List<TextMeshProUGUI> curSectors;

    private TextMeshProUGUI bestTotal;
    private List<TextMeshProUGUI> bestSectors;

    private TextMeshProUGUI prevTotal;
    private List<TextMeshProUGUI> prevSectors;

    void Awake()
    {
        curTotal = GetText(currentRow, "Total");
        curSectors = GetSectors(currentRow);

        bestTotal = GetText(bestRow, "Total");
        bestSectors = GetSectors(bestRow);

        prevTotal = GetText(previousRow, "Total");
        prevSectors = GetSectors(previousRow);
    }

    void OnEnable()
    {
        LapTimesSaver.OnLapSaved += OnLapSaved;
    }

    void OnDisable()
    {
        LapTimesSaver.OnLapSaved -= OnLapSaved;
    }

    private void OnLapSaved()
    {
        SetSectorTimesEmpty(curSectors);
        SetLapTimeEmpty(ref curTotal);

        UpdateBest();
        UpdatePrevious();
    }

    void Start()
    {
        UpdateCurrent();
        UpdateBest();
        UpdatePrevious();
    }

    void Update()
    {
        UpdateCurrent();
    }

    void UpdateCurrent()
    {
        float lap = LiveLapState.CurrentLapTime;
        curTotal.text = FormatLap(lap);

        for (int i = 0; i < curSectors.Count; i++)
        {
            if (i < LiveLapState.CurrentSectors.Count &&
                LiveLapState.CurrentSectors[i] > 0f)
            {
                curSectors[i].text = FormatSector(LiveLapState.CurrentSectors[i]);
            }
            else
            {
                curSectors[i].text = "--.---";
            }
        }
    }

    void UpdateBest()
    {
        TimeStats stats = LapTimesSaver.LoadAll(LiveLapState.FileName);

        if (stats.FastestTime <= 0f)
        {
            SetLapTimeEmpty(ref bestTotal);
            SetSectorTimesEmpty(bestSectors);
            return;
        }

        bestTotal.text = FormatLap(stats.FastestTime);

        for (int i = 0; i < bestSectors.Count; i++)
        {
            if (i < stats.SectorTimes.Count)
                bestSectors[i].text = FormatSector(stats.SectorTimes[i]);
            else
                bestSectors[i].text = "--.---";
        }
    }

    void UpdatePrevious()
    {
        float prev = LiveLapState.PreviousLapTime;

        if (prev <= 0f)
        {
            SetLapTimeEmpty(ref prevTotal);
            SetSectorTimesEmpty(prevSectors);
            return;
        }

        prevTotal.text = FormatLap(prev);


        for (int i = 0; i < prevSectors.Count; i++)
        {
            if (i < LiveLapState.PreviousSectors.Count)
                prevSectors[i].text = FormatSector(LiveLapState.PreviousSectors[i]);
            else
                prevSectors[i].text = "--.---";
        }
    }

    TextMeshProUGUI GetText(Transform parent, string name)
    {
        return parent.Find(name).GetComponent<TextMeshProUGUI>();
    }

    List<TextMeshProUGUI> GetSectors(Transform parent)
    {
        List<TextMeshProUGUI> list = new List<TextMeshProUGUI>();
        int i = 1;

        while (true)
        {
            Transform t = parent.Find("Sector" + i);
            if (t == null)
                break;

            list.Add(t.GetComponent<TextMeshProUGUI>());
            i++;
        }

        return list;
    }

    void SetSectorTimesEmpty(List<TextMeshProUGUI> texts)
    {
        foreach (var t in texts)
            t.text = "--.---";
    }

    void SetLapTimeEmpty(ref TextMeshProUGUI text)
    {
        text.text = "--.--.---";
    }

    string FormatLap(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;
        int millis = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:00}.{seconds:00}.{millis:000}";
    }

    string FormatSector(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        int millis = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{seconds:00}.{millis:000}";
    }
}