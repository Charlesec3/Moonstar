using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Manager : MonoBehaviour
{
    public static Manager instance;


    Player player;

    /*[SerializeField] GameObject gameOver;
    [SerializeField] GameObject congrats;

    public bool gameWon = false;*/

    public int numberOfKeys;
    public bool hasBossKey;

    [SerializeField] GameObject[] keyDisplay;
    [SerializeField] GameObject bossKeyDisplay;

    [SerializeField] GameObject startScreen;

    public int checkpointNum;
    public Transform[] checkpointPositions;
    [SerializeField] Vector3 startingPos;



    void Awake()
    {
        instance = this;

        Time.timeScale = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;

        if (File.Exists(Application.persistentDataPath + "/CheckpointData.txt") == true)
        {
            checkpointNum = Int32.Parse(File.ReadAllText(Application.persistentDataPath + "/CheckpointData.txt"));
        }
        else
        {
            checkpointNum = -1;
        }

       if(checkpointNum == -1)
        {
            /*Time.timeScale = 0;
            Player.instance.canMove = false;
            Player.instance.canCrouch = false;
            Player.instance.canAttack = false;

            //player.transform.position = startingPos;

            startScreen.SetActive(true);*/

            startGame();
        }
        else
        {
            player.transform.position = checkpointPositions[checkpointNum].position;
            startGame();
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*if(player.getCurrentHP() <= 0)
        {
            player.canMove = false;
            player.canAttack = false;
            player.canCrouch = false;

            gameOver.SetActive(true);
        }

        if(gameWon == true)
        {
            player.canMove = false;
            player.canAttack = false;
            player.canCrouch = false;

            congrats.SetActive(true);
        }*/

        displayKeys();
    }

    void displayKeys()
    {
        for(int i = 0; i < numberOfKeys; i++)
        {
            keyDisplay[i].SetActive(true);
        }

        for(int i = numberOfKeys; i < keyDisplay.Length; i++)
        {
            keyDisplay[i].SetActive(false);
        }


        if(hasBossKey == true)
        {
            bossKeyDisplay.SetActive(true);
        }
        else
        {
            bossKeyDisplay.SetActive(false);
        }
    }


    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void quitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void startGame()
    {
        Time.timeScale = 1;
        Player.instance.canMove = true;
        Player.instance.canCrouch = true;
        Player.instance.canAttack = true;

        startScreen.SetActive(false);
    }
}
