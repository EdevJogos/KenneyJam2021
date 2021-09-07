using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        AudioManager.PlaySFX(SFXOccurrence.BUTTON_SELECTED);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!GetComponent<Button>().interactable)
            return;

        AudioManager.PlaySFX(SFXOccurrence.BUTTON_PRESSED);
    }
}
