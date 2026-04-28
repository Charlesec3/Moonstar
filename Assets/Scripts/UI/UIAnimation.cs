using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class UIAnimation : MonoBehaviour
{
    public static UIAnimation instance;

    [Header("----------Pause Menu----------")]    
    [SerializeField] RectTransform pauseMenuGO;

    [SerializeField] Image pauseScreenChangeFilter;


    [SerializeField] float pauseIntroDuration;

    [SerializeField] RectTransform HPMarkers;

    [SerializeField] RectTransform controlsMenu;
    [SerializeField] RectTransform settingsMenu;

    [SerializeField] RectTransform questMenu;

    [SerializeField] Image pauseStateFilter;

    [Header("----------Main Menu----------")]
    [SerializeField] Image mainMenuFilter;
    [SerializeField] float mainMenuFilterFadeDuration;
    [SerializeField] float gameStartFadeDuration;
    [SerializeField] MainMenu mainMenuScript;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //openQuestMenus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#region Pause

    [ContextMenu("Pause Intro")]
    public void pauseIntro()
    {   
        //bottom
        DOTween.To(() => pauseMenuGO.offsetMin, x => pauseMenuGO.offsetMin = x, new Vector2(pauseMenuGO.offsetMin.x, 20.38785f), pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        
        //top
        DOTween.To(() => pauseMenuGO.offsetMax, x => pauseMenuGO.offsetMax = x, new Vector2(pauseMenuGO.offsetMax.x, -17.47525f), pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        HPMarkers.DOAnchorPosX(-200, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        pauseStateFilter.gameObject.SetActive(true);
        pauseStateFilter.DOFade(.7f,pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    [ContextMenu("Pause Outro")]
    public async Task pauseOutro()
    {
        //bottom
        DOTween.To(() => pauseMenuGO.offsetMin, x => pauseMenuGO.offsetMin = x, new Vector2(pauseMenuGO.offsetMin.x, 457.9315f), pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        pauseStateFilter.DOFade(0,pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        HPMarkers.DOAnchorPosX(50, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        questMenu.DOAnchorPosX(800,pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        
        //top
        await DOTween.To(() => pauseMenuGO.offsetMax, x => pauseMenuGO.offsetMax = x, new Vector2(pauseMenuGO.offsetMax.x, 420.0685f), pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();

        pauseMenuGO.gameObject.GetComponent<PauseMenu>().resetMenu(true);
    }


    async Task pauseFadeIn()
    {
        await pauseScreenChangeFilter.DOFade(1,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async void fadeToPauseControlsScreen()
    {
        await pauseFadeIn();

        pauseMenuGO.gameObject.GetComponent<PauseMenu>().getSettingsScreen().SetActive(false);
        pauseMenuGO.gameObject.GetComponent<PauseMenu>().getControlsScreen().SetActive(true);

        pauseScreenChangeFilter.DOFade(0,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public async void fadeToPauseSettingsScreen()
    {
        await pauseFadeIn();

        pauseMenuGO.gameObject.GetComponent<PauseMenu>().getSettingsScreen().SetActive(true);
        pauseMenuGO.gameObject.GetComponent<PauseMenu>().getControlsScreen().SetActive(false);

        pauseScreenChangeFilter.DOFade(0,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

#endregion

#region Quest    

    [ContextMenu("Quest Intro")]
    async Task questIntro()
    {
        //pause left
        DOTween.To(() => pauseMenuGO.offsetMin.x, x => pauseMenuGO.offsetMin = new Vector2(x, pauseMenuGO.offsetMin.y), -774.6996f, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        // pause right
        DOTween.To(() => pauseMenuGO.offsetMax.x, x => pauseMenuGO.offsetMax = new Vector2(x, pauseMenuGO.offsetMax.y), -821.3004f, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        //quest x
        await questMenu.DOAnchorPosX(0,pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }

    [ContextMenu("Quest Outro")]
    async Task questOutro()
    {
        //pause left
        DOTween.To(() => pauseMenuGO.offsetMin.x, x => pauseMenuGO.offsetMin = new Vector2(x, pauseMenuGO.offsetMin.y), 23.30042f, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);
        // pause right
        DOTween.To(() => pauseMenuGO.offsetMax.x, x => pauseMenuGO.offsetMax = new Vector2(x, pauseMenuGO.offsetMax.y), -23.30029f, pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true);

        //quest x
        await questMenu.DOAnchorPosX(800,pauseIntroDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async void openQuestMenu()
    {
        pauseStateFilter.gameObject.SetActive(false);

        questMenu.gameObject.SetActive(true);
        await questIntro();

        pauseMenuGO.gameObject.SetActive(false);

        //pauseMenuGO.gameObject.GetComponent<PauseMenu>().activateButton();
    }

    public async void closeQuestMenu()
    {
        pauseStateFilter.gameObject.SetActive(true);

        pauseMenuGO.gameObject.SetActive(true);
        await questOutro();

        questMenu.gameObject.SetActive(false);

        pauseMenuGO.gameObject.GetComponent<PauseMenu>().activateButton();
    }
#endregion

    
#region Main Menu    
    async Task mainMenuFadeIn()
    {
        await mainMenuFilter.DOFade(1,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async Task fadeToControlsScreen()
    {
        await mainMenuFadeIn();

        mainMenuScript.displayControls();

        await mainMenuFilter.DOFade(0,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async void fadeToCreditsScreen()
    {
        await mainMenuFadeIn();

        mainMenuScript.displayCredits();

        mainMenuFilter.DOFade(0,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    public async void fadeToMainMenuScreen()
    {
        await mainMenuFadeIn();

        mainMenuScript.back();

        mainMenuFilter.DOFade(0,mainMenuFilterFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    [ContextMenu("Game Start Fade In")]
    public void gameStartFadeIn()
    {
        Color tempColor = mainMenuFilter.color;
        tempColor.a = 1;
        //mainMenuFilter.color = tempColor;

        mainMenuFilter.DOFade(0,gameStartFadeDuration).SetEase(Ease.InOutSine).SetUpdate(true);
    }

#endregion

}
