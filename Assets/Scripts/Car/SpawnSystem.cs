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
}