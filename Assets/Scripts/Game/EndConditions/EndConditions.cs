using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class EndConditions : MonoBehaviour
{
    public static EndConditions instance;

    public GameObject gameOverScreen;
    public GameObject victoryScreen;


    private void Awake()
    {
        instance = this;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Time.timeScale = 0;

            Player.instance.canMove = false;
            Player.instance.canCrouch = false;
            Player.instance.canAttack = false;

            victoryScreen.SetActive(true);
        }
    }

    public void restart()
    {
        Time.timeScale = 1;

        int checkpointNum = -1;
        File.WriteAllText(Application.persistentDataPath + "/CheckpointData.txt", checkpointNum.ToString());
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void tryAgain()
    {
        Time.timeScale = 1;

        File.WriteAllText(Application.persistentDataPath + "/CheckpointData.txt", Manager.instance.checkpointNum.ToString());

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
