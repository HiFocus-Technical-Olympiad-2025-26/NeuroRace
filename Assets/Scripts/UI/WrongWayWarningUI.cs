using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrongWayWarningUI : MonoBehaviour
{
    [SerializeField] private BoolEventChannelSO wrongDirectionEvent;
    [SerializeField] private GameObject warningVisual;
    [SerializeField] private float blinkInterval = 0.4f;

    private Coroutine blinkCoroutine;

    void OnEnable()
    {
        wrongDirectionEvent.OnEventRaised += OnWrongDirectionChanged;
    }

    void OnDisable()
    {
        wrongDirectionEvent.OnEventRaised -= OnWrongDirectionChanged;
    }

    private void OnWrongDirectionChanged(bool isWrongWay)
    {
        if (isWrongWay)
        {
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(Blink());
        }
        else
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }

            warningVisual.SetActive(false);
        }
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            warningVisual.SetActive(!warningVisual.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}