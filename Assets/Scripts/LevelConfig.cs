using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;

    [Header("Car Setup")]
    public WheelSetup wheelSetup;
    public CarSettings carSettings;
}