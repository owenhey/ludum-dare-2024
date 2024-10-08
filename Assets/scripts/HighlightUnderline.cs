using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightUnderline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public Image image;

    public float width;

    private void OnEnable() {
        image.rectTransform.sizeDelta = new Vector2(0, image.rectTransform.sizeDelta.y);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Sound.I.PlaySwish();
        
        image.rectTransform.DOKill();
        image.rectTransform.DOSizeDelta(new Vector2(width, image.rectTransform.sizeDelta.y), .08f);
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        Sound.I.PlayKnock();
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        image.rectTransform.DOKill();
        image.rectTransform.DOSizeDelta(new Vector2(0, image.rectTransform.sizeDelta.y), .08f);
    }
}
