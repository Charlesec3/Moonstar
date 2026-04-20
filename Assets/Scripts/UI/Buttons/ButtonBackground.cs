using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class ButtonBackground : ButtonAnimationBase
{
    [SerializeField] Image image;

    [SerializeField] float duration;


    void Awake()
    {
        if(image == null)
        {
            image = this.gameObject.GetComponent<Image>();
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        image.DOFade(1, duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        image.DOFade(0, duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }
}
