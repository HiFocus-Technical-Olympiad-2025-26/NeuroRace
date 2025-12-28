using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    public CarSettings playerCarSettings;
    public CarSettings AICarSettings;
    public WheelSetup playerWheelSetup;
    public WheelSetup AIWheelSetup;
    public bool ShowNeuroObstacle;
    public int StartPosition;
    public bool RandomizeStartPosition;
    public int NumOfAIs;
    public int skinIndex;
}