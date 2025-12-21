using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIThemeManager : MonoBehaviour
{
    public static UIThemeManager Instance;

    public List<UITheme> themes = new List<UITheme>();
    public int currentThemeIndex { private set; get; } = 0;

    public UITheme CurrentTheme => themes[currentThemeIndex];

    public event Action OnThemeChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(InputManager.Instance.Menu.ConsumeNextTheme())
            NextTheme();
    }

    public void SetTheme(int index)
    {
        index = (index + themes.Count) % themes.Count;

        if (index < 0 || index >= themes.Count)
        {
            Debug.LogWarning("Theme index out of range");
            return;
        }

        currentThemeIndex = index;
        OnThemeChanged?.Invoke();
    }

    public void NextTheme()
    {
        SetTheme(currentThemeIndex + 1);
    }

    public void PreviousTheme()
    {
        SetTheme(currentThemeIndex - 1);
    }
}
