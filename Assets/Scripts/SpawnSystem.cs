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
        foreach (Transform t in trackPointsParent)
            trackPoints.Add(t);

        foreach (Transform t in startPointsParent)
            startPoints.Add(t);
    }

    public void SpawnCarOnStart()
    {
        //Debug.Log("Start called");
        Spawn(StartPoint);
    }

    public void SpawnCarOnSpecificStart(int positionIndex)
    {
        car = startPoints[positionIndex];
    }

    public void SpawnCarAtNearestPoint()
    {
        //Debug.Log("Nearest called");

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
        Rigidbody rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void Spawn(int indexOfPoint)
    {
        //Debug.Log("SPAWN CALL: index=" + indexOfPoint);

        StopCar();

        if (indexOfPoint > trackPoints.Count || trackPoints.Count == 0)
            return;

        Quaternion rotation;
        if (indexOfPoint == trackPoints.Count - 1)
            rotation = Quaternion.LookRotation(trackPoints[0].position - trackPoints[indexOfPoint].position, Vector3.up);
        else
            rotation = Quaternion.LookRotation(trackPoints[indexOfPoint + 1].position - trackPoints[indexOfPoint].position, Vector3.up);

        car.position = trackPoints[indexOfPoint].position;
        car.rotation = rotation;
    }
}