using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject controlsScreen;
    [SerializeField] GameObject creditsScreen;


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
    }

    public void displayCredits()
    {
        mainMenu.SetActive(false);

        creditsScreen.SetActive(true);
    }

    public void back()
    {
        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        mainMenu.SetActive(true);
    }
}
