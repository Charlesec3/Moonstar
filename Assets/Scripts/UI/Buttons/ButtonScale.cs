using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Threading.Tasks;
using UnityEngine.UI;

public class ButtonScale : ButtonAnimationBase
{
    const float originalScale = 1;

    [SerializeField] float hoverScale = 1.1f;
    [SerializeField] float clickScale = 0.9f;

    [SerializeField] float duration = 0.2f;


    public override void OnPointerEnter(PointerEventData eventData)
    {
       this.transform.DOScale(hoverScale,duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        this.transform.DOScale(originalScale,duration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        scaleButtonClick();
    }

    async void scaleButtonClick()
    {
        await scaleDown();
        
        if(this != null)
        {
            this.transform.DOScale(originalScale,duration).SetEase(Ease.InOutSine).SetUpdate(true);
        }
    }

    async Task scaleDown()
    {
        await this.transform.DOScale(clickScale,duration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }
}
