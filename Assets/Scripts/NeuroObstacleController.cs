using NextMind.NeuroTags;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NeuroTag))]
public class NeuroObstacleController : MonoBehaviour
{
    [Header("Neuro")]
    [SerializeField] private float disappearThreshold = 0.85f;

    [Header("References")]
    [SerializeField] private GameObject visual;
    [SerializeField] private Collider solidCollider;

    [Header("Penalty")]
    [SerializeField] private float penaltyDuration = 0.5f;

    [Header("AI")]
    [SerializeField] private string aiTag = "AI";

    private bool isCleared = false;
    private bool isPenaltyRunning = false;

    private void Start()
    {
        IgnoreAICollisions();
    }

    public void OnConfidenceChange(float value)
    {
        if (isCleared) return;

        if (value >= disappearThreshold)
        {
            ClearObstacle();
        }
    }

    private void ClearObstacle()
    {
        isCleared = true;

        visual.SetActive(false);
        solidCollider.enabled = false;
    }

    public void PlayerHit()
    {
        if (isCleared || isPenaltyRunning) return;

        StartCoroutine(PenaltyRoutine());
    }

    private IEnumerator PenaltyRoutine()
    {
        isPenaltyRunning = true;

        solidCollider.enabled = true;

        yield return new WaitForSeconds(penaltyDuration);

        ClearObstacle();
    }

    public void IgnoreAICollisions()
    {
        var aiObjects = GameObject.FindGameObjectsWithTag(aiTag);

        foreach (var ai in aiObjects)
        {
            var colliders = ai.GetComponentsInChildren<Collider>();

            foreach (var col in colliders)
            {
                Physics.IgnoreCollision(solidCollider, col, true);
            }
        }

        Debug.Log($"detected {aiObjects.Length} AI cars");
    }
}
