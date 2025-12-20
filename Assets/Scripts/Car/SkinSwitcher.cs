using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> skins = new List<GameObject>();
    [SerializeField] private int currentSkinIndex = 0;

    void Start()
    {
        if (skins.Count == 0)
        {
            Debug.Log("Skin list is empty");
            return;
        }

        if (currentSkinIndex < 0 || currentSkinIndex >= skins.Count)
            currentSkinIndex = (currentSkinIndex + skins.Count) % skins.Count;

        for (int i = 0; i < skins.Count; i++)
        {
            skins[i].SetActive(i == currentSkinIndex);
        }
    }

    void Update()
    {
        var input = InputManager.Instance.GamePlay;
        if (input.ConsumeSkin())
            SetSkinIndex(currentSkinIndex + 1);
    }

    public void SetSkinIndex(int newIndex)
    {
        if (skins.Count == 0)
        {
            Debug.Log("Skin list is empty");
            return;
        }

        skins[currentSkinIndex].SetActive(false);
        currentSkinIndex = (newIndex + skins.Count) % skins.Count;
        skins[currentSkinIndex].SetActive(true);
    }
}
