using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIThemeElement : MonoBehaviour
{
    public enum ElementType
    {
        Image,
        Text,
        Button,
        Toggle,
        Light,
        Renderer
    }

    public enum ThemeColorType
    {
        Background,
        Accent1,
        Accent2,
        Text,
        TextSecondary
    }

    public ElementType type;
    public ThemeColorType colorType;

    private void OnEnable()
    {
        if (UIThemeManager.Instance != null)
        {
            UIThemeManager.Instance.OnThemeChanged += ApplyTheme;
            ApplyTheme();
        }
    }

    private void OnDisable()
    {
        if (UIThemeManager.Instance != null)
            UIThemeManager.Instance.OnThemeChanged -= ApplyTheme;
    }

    private Color GetColor(UITheme theme)
    {
        return colorType switch
        {
            ThemeColorType.Background => theme.backgroundColor,
            ThemeColorType.Accent1 => theme.accentColor,
            ThemeColorType.Accent2 => theme.accentColor2,
            ThemeColorType.Text => theme.textColor,
            ThemeColorType.TextSecondary => theme.secondaryTextColor,
            _ => Color.magenta
        };
    }

    private void ApplyTheme()
    {
        UITheme theme = UIThemeManager.Instance.CurrentTheme;
        Color chosen = GetColor(theme);

        //Debug.Log($"Color of {type.ToString()}: {chosen.ToString()}");

        switch (type)
        {
            case ElementType.Image:
                if (TryGetComponent<Image>(out var img))
                    img.color = chosen;
                break;

            case ElementType.Text:
                if (TryGetComponent<TMP_Text>(out var text))
                    text.color = chosen;
                break;

            case ElementType.Button:
                if (TryGetComponent<Button>(out var btn))
                {
                    var c = btn.colors;
                    c.normalColor = chosen;
                    c.highlightedColor = chosen;
                    c.pressedColor = chosen;
                    btn.colors = c;

                    var btnTxt = GetComponentInChildren<TMP_Text>();
                    if (btnTxt != null)
                        btnTxt.color = theme.textColor;
                }
                break;

            case ElementType.Toggle:
                if (TryGetComponent<Toggle>(out var toggle))
                {
                    if (toggle.graphic != null)
                        toggle.graphic.color = chosen;

                    if (toggle.targetGraphic != null)
                        toggle.targetGraphic.color = chosen;

                    var tgTxt = GetComponentInChildren<TMP_Text>();
                    if (tgTxt != null)
                        tgTxt.color = theme.textColor;
                }
                break;

            case ElementType.Light:
                if (TryGetComponent<Light>(out var light))
                    light.color = chosen;
                break;

            case ElementType.Renderer:
                if (TryGetComponent<Renderer>(out var rend))
                {
                    if (rend.material.HasProperty("_Color"))
                        rend.material.color = chosen;
                }
                break;
        }
    }
}