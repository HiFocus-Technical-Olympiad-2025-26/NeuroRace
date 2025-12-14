using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIDirection
{
    Left,
    Right
}

public class AICheckpoint : MonoBehaviour
{
    public int checkpoint_id;
    public AIDirection direction;
    public bool ignoreRules;
    public bool removeCheckpointEffects;
}
