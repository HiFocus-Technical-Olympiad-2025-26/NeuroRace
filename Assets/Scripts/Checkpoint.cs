using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Instruction
{
    int checkpointid;
    int rotation;
}

public class Checkpoint : MonoBehaviour
{
    public Instruction instruction { get; private set; }
}
