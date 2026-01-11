using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkipIfDisabledSelectable : MonoBehaviour, ISelectHandler
{
    [Header("Focus Redirect")]
    public Selectable redirectUp;
    public Selectable redirectDown;

    public void OnSelect(BaseEventData eventData)
    {
        Selectable selectable = GetComponent<Selectable>();
        if (!selectable.interactable)
        {
            Vector2 lastDir = InputManager.Instance.Menu.Direction;

            Selectable target = null;

            if (lastDir.y > 0f)
                target = redirectUp;
            else if (lastDir.y < 0f)
                target = redirectDown;
            else
                target = redirectDown;

            if (target != null && EventSystem.current.currentSelectedGameObject != target.gameObject)
                StartCoroutine(SelectNextFrame(target));
        }
    }

    private IEnumerator SelectNextFrame(Selectable target)
    {
        yield return null; // wait 1 frame
        EventSystem.current.SetSelectedGameObject(target.gameObject);
    }
}
