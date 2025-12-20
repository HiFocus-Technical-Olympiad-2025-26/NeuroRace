using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFail : MonoBehaviour
{
    public int SpawnPointIndex = 186;
    [SerializeField] private SpawnSystem spawnSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            if (spawnSystem != null)
            {
                Transform root = other.transform.root;
                spawnSystem.SpawnAtSpecificTrackPoint(root, SpawnPointIndex);
            }
            else
            {
                Debug.LogWarning("SpawnSystem is null!", other);
            }
        }
    }
}
