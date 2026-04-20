using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;
using DG.Tweening;
using System.Threading.Tasks;

public class VolumeSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Slider slider;

    [SerializeField] TextMeshProUGUI valueDispalyText;

    [SerializeField] GameObject displayGameobject;

    bool hovering;

    [SerializeField] float fadeDuration;

    [SerializeField] float scale;

    bool closeCheck = false;

    
    void Start()
    {
        slider = this.gameObject.GetComponent<Slider>();

        displayGameobject.SetActive(false);

        valueDispalyText.text =  Mathf.RoundToInt(slider.value).ToString();
    }

    void Update()
    {
        if((EventSystem.current.currentSelectedGameObject == slider.gameObject || hovering == true) && displayGameobject.gameObject.activeSelf == false)  
        {
            openDisplay();
        }
        else if(closeCheck == false && EventSystem.current.currentSelectedGameObject != slider.gameObject && hovering == false && displayGameobject.gameObject.activeSelf == true)
        {
            closeDisplay();
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;

        //openDisplay();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;

        //closeDisplay();
    }

    public void updateValueDisplay()
    {
        valueDispalyText.text =  Mathf.RoundToInt(slider.value).ToString();
    }

    async void closeDisplay()
    {
        closeCheck = true;
        await fadeOut();
        displayGameobject.SetActive(false);
        closeCheck = false;
    }

    void openDisplay()
    {
        displayGameobject.SetActive(true);
        fadeIn();
    }

    void fadeIn()
    {
        //displayGameobject.GetComponent<Image>().DOFade(.5f, fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        displayGameobject.GetComponent<RectTransform>().DOScale(1,fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        displayGameobject.transform.GetComponentInChildren<RectTransform>().DOScale(1,fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    async Task fadeOut()
    {
        //await displayGameobject.GetComponent<Image>().DOFade(0, fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
        displayGameobject.transform.GetComponentInChildren<RectTransform>().DOScale(scale,fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        await displayGameobject.GetComponent<RectTransform>().DOScale(scale,fadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }
}
