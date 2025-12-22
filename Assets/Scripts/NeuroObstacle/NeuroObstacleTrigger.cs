using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroObstacleTrigger : MonoBehaviour
{
    public enum TriggerType
    {
        Hit,
        Reset
    }

    [SerializeField] private NeuroObstacleController controller;
    [SerializeField] private TriggerType triggerType;
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        switch (triggerType)
        {
            case TriggerType.Hit:
                controller.PlayerHit();
                break;

            case TriggerType.Reset:
                controller.ResetObstacle();
                break;
        }
    }
}
