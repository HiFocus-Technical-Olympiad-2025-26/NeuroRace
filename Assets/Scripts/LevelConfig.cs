using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;

    [Header("Player car Setup")]
    public WheelSetup playerWheelSetup;
    public CarSettings playerCarSettings;

    [Header("AI car Setup")]
    public WheelSetup AIWheelSetup;
    public CarSettings AICarSettings;
}