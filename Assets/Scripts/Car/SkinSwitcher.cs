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

    private void SetSkinIndex(int newIndex)
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

    public void NextSkin()
    {
        SetSkinIndex(currentSkinIndex + 1);
    }

    public void PreviousSkin()
    {
        SetSkinIndex(currentSkinIndex - 1);
    }

    public void SetRandomSkin()
    {
        if (skins.Count == 0)
            return;

        SetSkinIndex(Random.Range(0, skins.Count * 5) % skins.Count);
    }
}
