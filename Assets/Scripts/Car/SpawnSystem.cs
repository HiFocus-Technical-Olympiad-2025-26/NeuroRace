using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform trackPointsParent;
    [SerializeField] private Transform startPointsParent;

    private List<Transform> trackPoints = new List<Transform>();
    private List<Transform> startPoints = new List<Transform>();

    void Awake()
    {
        if (trackPointsParent != null)
        {
            foreach (Transform t in trackPointsParent)
                trackPoints.Add(t);
        }

        if (startPointsParent != null)
        {
            foreach (Transform t in startPointsParent)
                startPoints.Add(t);
        }
    }

    public void SpawnCarOnSpecificStart(Transform car, int positionIndex)
    {
        if (car == null || positionIndex < 0 || positionIndex >= startPoints.Count)
            return;

        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb == null)
            return;

        rb.isKinematic = true;
        car.SetPositionAndRotation(startPoints[positionIndex].position, startPoints[positionIndex].rotation);
        rb.isKinematic = false;
        ResetRigidbody(car);
    }

    public void SpawnCarAtNearestPoint(Transform car)
    {
        if (trackPoints.Count == 0)
            return;

        Transform nearest = trackPoints[0];
        int nearestIndex = 0;
        float minDist = Vector3.Distance(car.position, nearest.position);

        for (int i = 1; i < trackPoints.Count; i++)
        {
            float dist = Vector3.Distance(car.position, trackPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = trackPoints[i];
                nearestIndex = i;
            }
        }

        this.SpawnAtSpecificTrackPoint(car, nearestIndex);
    }

    public void ResetRigidbody(Transform car)
    {
        if (car == null)
            return;

        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.Sleep();
            rb.WakeUp();
        }
    }

    public void SpawnAtSpecificTrackPoint(Transform car, int indexOfPoint)
    {
        if (car == null)
            return;

        Rigidbody rb = car.GetComponent<Rigidbody>();

        if (indexOfPoint < 0 || indexOfPoint >= trackPoints.Count || trackPoints.Count == 0)
            return;

        Transform current = trackPoints[indexOfPoint];
        Transform next = (indexOfPoint == trackPoints.Count - 1) ? trackPoints[0] : trackPoints[indexOfPoint + 1];

        Quaternion rotation = Quaternion.LookRotation(next.position - current.position, Vector3.up);

        if (rb != null)
            rb.isKinematic = true;

        car.SetPositionAndRotation(current.position, rotation);

        if (rb != null)
            rb.isKinematic = false;

        ResetRigidbody(car);
    }

    public bool IsCarGoingWrongDirection(Transform car, float dotThreshold = 0.0f)
    {
        if (car == null || trackPoints.Count < 2)
            return false;

        int nearestIndex = 0;
        float minDist = Vector3.SqrMagnitude(car.position - trackPoints[0].position);

        for (int i = 1; i < trackPoints.Count; i++)
        {
            float dist = Vector3.SqrMagnitude(car.position - trackPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestIndex = i;
            }
        }

        int prevIndex = (nearestIndex - 1 + trackPoints.Count) % trackPoints.Count;
        int nextIndex = (nearestIndex + 1) % trackPoints.Count;

        Vector3 pPrev = trackPoints[prevIndex].position;
        Vector3 pCurr = trackPoints[nearestIndex].position;
        Vector3 pNext = trackPoints[nextIndex].position;

        // segment vectors
        Vector3 prevSeg = pCurr - pPrev;
        Vector3 nextSeg = pNext - pCurr;

        // vector from segment start to car
        Vector3 toCarPrev = car.position - pPrev;
        Vector3 toCarNext = car.position - pCurr;

        float tPrev = Vector3.Dot(toCarPrev, prevSeg) / prevSeg.sqrMagnitude;
        float tNext = Vector3.Dot(toCarNext, nextSeg) / nextSeg.sqrMagnitude;

        bool onPrevSegment = tPrev >= 0f && tPrev <= 1f;
        bool onNextSegment = tNext >= 0f && tNext <= 1f;

        Vector3 trackDir;
        if (onPrevSegment && !onNextSegment)
            trackDir = prevSeg.normalized;
        else if (onNextSegment && !onPrevSegment)
            trackDir = nextSeg.normalized;
        else
            trackDir = (Vector3.SqrMagnitude(toCarPrev) < Vector3.SqrMagnitude(toCarNext)) ? 
                    prevSeg.normalized : nextSeg.normalized;

        float dot = Vector3.Dot(car.forward.normalized, trackDir);
        return dot < dotThreshold;
    }
}