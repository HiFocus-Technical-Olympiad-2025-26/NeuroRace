using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UITheme", menuName = "Game/UI Theme")]
public class UITheme : ScriptableObject
{
    public Color backgroundColor;
    public Color accentColor;
    public Color accentColor2;
    public Color textColor;
    public Color secondaryTextColor;
}