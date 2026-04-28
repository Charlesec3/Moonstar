using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject creditsScreen;

    [SerializeField] Button newGameBtn;

    [SerializeField] Button controlsBackBtn;
    [SerializeField] Button creditsBackBtn;


    [SerializeField] GameObject keyboardControls;
    [SerializeField] GameObject gamepadControls;
    [SerializeField] TextMeshProUGUI switchControlsText;


    void Start()
    {
        Time.timeScale = 1;
        
        if (Gamepad.all.Count >= 1)
        {
            newGameBtn.Select();
        }

        //UIAnimation.instance.gameStartFadeIn();
    }

    public void newGame()
    {
        if (File.Exists(Application.persistentDataPath + "/CheckpointData.txt") == true)
        {
            File.Delete(Application.persistentDataPath + "/CheckpointData.txt");
        }

        SceneManager.LoadScene(1);
    }

    public void continueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void displayControls()
    {
        mainMenu.SetActive(false);

        controlsScreen.SetActive(true);

        controlsBackBtn.Select();

        if (Gamepad.all.Count >= 1)
        {
            gamepadControls.SetActive(true);
            keyboardControls.SetActive(false);

            switchControlsText.text = "View Keyboard Controls";
        }
        else if(Gamepad.all.Count == 0)
        {
            gamepadControls.SetActive(false);
            keyboardControls.SetActive(true);

            switchControlsText.text = "View Gamepad Controls";
        }
    }

    public void displayCredits()
    {
        mainMenu.SetActive(false);

        creditsScreen.SetActive(true);

        creditsBackBtn.Select();
    }

    public void back()
    {
        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        mainMenu.SetActive(true);

        newGameBtn.Select();
    }

    public void switchControls()
    {
        if(switchControlsText.text == "View Keyboard Controls")
        {
            switchControlsText.text = "View Gamepad Controls";
        }
        else
        {
            switchControlsText.text = "View Keyboard Controls";
        }

        gamepadControls.SetActive(!gamepadControls.activeSelf);
        keyboardControls.SetActive(!keyboardControls.activeSelf);
    }
}
