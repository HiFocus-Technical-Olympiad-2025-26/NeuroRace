using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.Image;

public class SpawnSystem : MonoBehaviour
{
    [SerializeField] private Transform car;
    [SerializeField] private Transform trackPointsParent;
    [SerializeField] private int StartPoint = 0;
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

    public void SpawnCarOnStart()
    {
        Spawn(StartPoint);
    }

    public void SpawnCarOnSpecificStart(int positionIndex)
    {
        car.position = startPoints[positionIndex].position;
        car.rotation = startPoints[positionIndex].rotation;
    }

    public void SpawnCarAtNearestPoint()
    {
        if (trackPoints.Count == 0)
            return;

        Transform nearest = trackPoints[0];
        float minDist = Vector3.Distance(car.position, nearest.position);

        for (int i = 1; i < trackPoints.Count; i++)
        {
            float dist = Vector3.Distance(car.position, trackPoints[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = trackPoints[i];
            }
        }

        int index = trackPoints.IndexOf(nearest);

        this.Spawn(index);
    }

    public void StopCar()
    {
        if (car == null)
            return;

        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Spawn(int indexOfPoint)
    {
        StopCar();

        if (indexOfPoint >= trackPoints.Count || trackPoints.Count == 0)
            return;

        Transform current = trackPoints[indexOfPoint];
        Transform next = (indexOfPoint == trackPoints.Count - 1) ? trackPoints[0] : trackPoints[indexOfPoint + 1];

        Quaternion rotation = Quaternion.LookRotation(next.position - current.position, Vector3.up);

        car.position = current.position;
        car.rotation = rotation;
    }
}