using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EffectsHelper.SFX("UI/onButtonClick");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EffectsHelper.SFX("UI/onButtonEnter");
        transform.DOScale(1.1f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EffectsHelper.SFX("UI/onButtonExit");
        transform.DOScale(1, 0.5f);
    }
}
