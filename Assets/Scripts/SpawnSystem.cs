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

    private List<Transform> points = new List<Transform>();

    void Awake()
    {
        foreach (Transform t in trackPointsParent)
        {
            points.Add(t);
        }
    }

    public void SpawnCarOnStart()
    {
        //Debug.Log("Start called");
        Spawn(StartPoint);
    }

    public void SpawnCarAtNearestPoint()
    {
        //Debug.Log("Nearest called");

        if (points.Count == 0)
            return;

        Transform nearest = points[0];
        float minDist = Vector3.Distance(car.position, nearest.position);

        for (int i = 1; i < points.Count; i++)
        {
            float dist = Vector3.Distance(car.position, points[i].position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = points[i];
            }
        }

        int index = points.IndexOf(nearest);
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

        if (indexOfPoint > points.Count || points.Count == 0)
            return;

        Quaternion rotation;
        if (indexOfPoint == points.Count - 1)
            rotation = Quaternion.LookRotation(points[0].position - points[indexOfPoint].position, Vector3.up);
        else
            rotation = Quaternion.LookRotation(points[indexOfPoint + 1].position - points[indexOfPoint].position, Vector3.up);

        car.position = points[indexOfPoint].position;
        car.rotation = rotation;
    }
}