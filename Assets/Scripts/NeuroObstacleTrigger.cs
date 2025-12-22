using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuroObstacleTrigger : MonoBehaviour
{
    [SerializeField] private NeuroObstacleController controller;
    [SerializeField] private string PlayerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(PlayerTag)) return;

        controller.PlayerHit();
    }
}
