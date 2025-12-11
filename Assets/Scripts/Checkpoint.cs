using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right
}

public class Checkpoint : MonoBehaviour
{
    public int checkpoint_id;
    public Direction direction;
    public bool ignoreRules;
    public bool removeCheckpointEffects;
}
