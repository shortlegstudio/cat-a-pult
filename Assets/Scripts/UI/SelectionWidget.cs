using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionWidget : MonoBehaviour, ISelectHandler, IDeselectHandler, UnityEngine.EventSystems.IPointerExitHandler, IPointerEnterHandler

{
    public GameObject selectionImage;
    public void OnSelect(BaseEventData eventData)
    {
        if(selectionImage != null)
            selectionImage.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(selectionImage != null)
            selectionImage.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectionImage != null)
            selectionImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectionImage != null)
            selectionImage.SetActive(true);
    }
}
