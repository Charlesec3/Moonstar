using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DirectionAssistArrow : MonoBehaviour
{
    [SerializeField] RectTransform arrow;

    [SerializeField] bool activate;

    [SerializeField] float duration = .3f;


    void uiAnimation(bool activate)
    {
        if(activate == true)
        {
            arrow.DOAnchorPosX(-45, duration).SetEase(Ease.InOutSine).SetUpdate(true);
        }
        else
        {
            arrow.DOAnchorPosX(45, duration).SetEase(Ease.InOutSine).SetUpdate(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Manager.instance.checkpointNum == 1)
            {
                uiAnimation(activate);
            }
        }
    }
}
