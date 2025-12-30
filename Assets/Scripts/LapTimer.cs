using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    public List<Collider> sectorTriggers = new List<Collider>();

    private float lapStartTime;
    private float sectorStartTime;
    private int nextSectorIndex = -1;

    private Dictionary<int, float> sectorTimes = new Dictionary<int, float>();

    void Start()
    {
        for (int i = 0; i < sectorTriggers.Count; i++)
        {
            SectorTrigger trigger = sectorTriggers[i].gameObject.AddComponent<SectorTrigger>();
            trigger.parentTimer = this;
            trigger.myIndex = i;
        }
    }

    void Update()
    {
        if (nextSectorIndex == -1)
            return;

        LiveLapState.CurrentLapTime = Round3(Time.time - lapStartTime);
        LiveLapState.CurrentSectors[LiveLapState.CurrentSectorIndex] = Round3(Time.time - sectorStartTime);
    }

    public void ResetTimer()
    {
        nextSectorIndex = -1;
        LiveLapState.CurrentLapTime = 0f;
        LiveLapState.CurrentSectors.Clear();
        LiveLapState.CurrentSectorIndex = 0;
    }

    private void OnSectorTriggered(int index)
    {
        if (nextSectorIndex == -1)
        {
            lapStartTime = Time.time;
            sectorStartTime = Time.time;

            LiveLapState.CurrentLapTime = 0f;
            LiveLapState.CurrentSectors.Clear();

            for (int i = 0; i < sectorTriggers.Count; i++)
                LiveLapState.CurrentSectors.Add(0f);

            LiveLapState.CurrentSectorIndex = 0;

            nextSectorIndex = 1;
            return;
        }

        if (index != nextSectorIndex)
            return;

        float now = Time.time;
        float sectorTime = Round3(now - sectorStartTime);

        sectorTimes[index] = sectorTime;

        //Debug.Log($"Sector {index} time: {sectorTime}s");

        sectorStartTime = now;
        LiveLapState.CurrentSectorIndex = (LiveLapState.CurrentSectorIndex + 1) % sectorTriggers.Count;
        float currentLapTime = Round3(now - lapStartTime);


        if (index == 0 && nextSectorIndex == 0 && now != lapStartTime)
        {
            //Debug.Log($"LAP COMPLETED: {currentLapTime}s");

            // Live lap state update
            LiveLapState.PreviousLapTime = currentLapTime;
            LiveLapState.PreviousSectors = new List<float>(sectorTimes.Count);
            for (int i = 0; i < sectorTimes.Count; i++)
                LiveLapState.PreviousSectors.Add(sectorTimes[i]);

            for (int i = 0; i < sectorTimes.Count; i++)
                LiveLapState.CurrentSectors[i] = 0f;

            // Save lap time
            List<float> sectorsCopy = new List<float>(sectorTimes.Values);
            LapTimesSaver.SaveLapTime(currentLapTime, sectorsCopy);

            // Reset for next lap
            lapStartTime = Time.time;
            sectorStartTime = Time.time;

            nextSectorIndex = 1;
            LiveLapState.CurrentSectorIndex = 0;

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

    float Round3(float v) => Mathf.Round(v * 1000f) / 1000f;
}