using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIThemeManager : MonoBehaviour
{
    public static UIThemeManager Instance;

    public List<UITheme> themes = new List<UITheme>();
    public int currentThemeIndex = 0;

    public UITheme CurrentTheme => themes[currentThemeIndex];

    public event Action OnThemeChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void SetTheme(int index)
    {
        if (index < 0 || index >= themes.Count)
        {
            Debug.LogWarning("Theme index out of range");
            return;
        }

        currentThemeIndex = index;
        OnThemeChanged?.Invoke();
    }
}
