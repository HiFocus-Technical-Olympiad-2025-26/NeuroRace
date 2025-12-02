using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LapTimer : MonoBehaviour
{
    public  List<Collider> sectorTriggers = new List<Collider>();

    private float lapStartTime;
    private float sectorStartTime;
    private int nextSectorIndex = 0;

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

    private void OnSectorTriggered(int index)
    {
        if (index != nextSectorIndex)
            return;

        float now = Time.time;
        float sectorTime = now - sectorStartTime;

        sectorTimes[index] = sectorTime;

        Debug.Log($"Sector {index} time: {sectorTime:F3}s");

        sectorStartTime = now;

        if (index == 0 && nextSectorIndex == 0 && now != lapStartTime)
        {
            float lapTime = now - lapStartTime;
            Debug.Log($"LAP COMPLETED: {lapTime:F3}s");

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
}