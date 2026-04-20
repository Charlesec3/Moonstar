using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ControlsToastMessage : MonoBehaviour
{
    [SerializeField] RectTransform messageGameobject;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] float movementDuration;
    [SerializeField] float waitTime;

    //[SerializeField] bool displayUpdated = true;
    [SerializeField]bool usingController;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        startMessage();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count >= 1 && usingController == false)
        {
            updateMessage();
        }
        else if(Gamepad.all.Count == 0 && usingController == true)
        {
           updateMessage();
        }
    }

    async Task messageAppear()
    {
        //bottom
        //DOTween.To(() => messageGameobject.offsetMin, x => messageGameobject.offsetMin = x, new Vector2(messageGameobject.offsetMin.x, 410f), movementDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        
        //top
        //await DOTween.To(() => messageGameobject.offsetMax, x => messageGameobject.offsetMax = x, new Vector2(messageGameobject.offsetMax.x, -3f), movementDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
        await messageGameobject.DOAnchorPosY(-25, movementDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();

    }

    public async void switchToControlsScreenWithToast()
    {
        await UIAnimation.instance.fadeToControlsScreen();

        await messageAppear();

        StartCoroutine(retractMessage());
    }

    async void updateMessage()
    {
        usingController = !usingController;

        if (Gamepad.all.Count >= 1)
        {
            messageText.text = "Contoller Is Connected";
        }
        else if(Gamepad.all.Count == 0)
        {
            messageText.text = "Contoller Is Not Connected";
        }

        await messageAppear();

        StartCoroutine(retractMessage());
    }

    async void startMessage()
    {
        if (Gamepad.all.Count >= 1)
        {
            usingController = true;
            messageText.text = "Contoller Is Connected";

            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
            {
                Debug.Log("TOAST: on main menu");
                await messageAppear();

                StartCoroutine(retractMessage());
            }
        
        }
        else if(Gamepad.all.Count == 0)
        {
            usingController = false;
            messageText.text = "Contoller Is Not Connected";
        }
    }

    IEnumerator retractMessage()
    {
        yield return new WaitForSeconds(waitTime);

        //bottom
        //DOTween.To(() => messageGameobject.offsetMin, x => messageGameobject.offsetMin = x, new Vector2(messageGameobject.offsetMin.x, 445f), movementDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        
        //top
        //DOTween.To(() => messageGameobject.offsetMax, x => messageGameobject.offsetMax = x, new Vector2(messageGameobject.offsetMax.x, 32f), movementDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        messageGameobject.DOAnchorPosY(20, movementDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }
}
