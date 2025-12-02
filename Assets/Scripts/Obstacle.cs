using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NextMind.NeuroTags;

public class Obstacle : MonoBehaviour
{
    [Range(0f, 5f)] [SerializeField] private float ConfidenceThreshold = 0.5f;

    void Start()
    {
        NeuroTag tag = GetComponent<NeuroTag>();

        if (tag == null)
        {
            Debug.LogError("NeuroTag missing!");
            return;
        }

        tag.onStimulationStateUpdated.AddListener(HandleStateUpdate);
    }

    private void HandleStateUpdate(GameObject obj, float confidence)
    {
        if (confidence >= ConfidenceThreshold)
        {
            Debug.Log("Destroyed obstacle by gaze: " + confidence.ToString());
            //Destroy(gameObject);
        }
    }

    /*void OnMouseDown()
    {
        Debug.Log("Destroyed obstacle by mouse click");
        Destroy(gameObject);
    }*/
}
