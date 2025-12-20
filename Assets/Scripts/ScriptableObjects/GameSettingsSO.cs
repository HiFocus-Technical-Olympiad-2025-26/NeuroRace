using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    public CarSettings playerCarSettings;
    public CarSettings AICarSettings;
    public WheelSetup playerWheelSetup;
    public WheelSetup AIWheelSetup;
    public int StartPosition;
    public int NumOfAIs;
}