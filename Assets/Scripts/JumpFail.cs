using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFail : MonoBehaviour
{
    public int SpawnPointIndex = 186;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            SpawnSystem ss = other.GetComponentInParent<SpawnSystem>();

            if (ss != null)
            {
                ss.Spawn(SpawnPointIndex);
            }
            else
            {
                Debug.LogWarning("SpawnSystem component not found on object!", other);
            }
        }
    }
}
