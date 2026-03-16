using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType { Locked, Trigger, Enemy, Boss };
    public DoorType type;
    
    Manager manager;

    public bool triggered = false;
    [SerializeField] GameObject[] enemies;

    [SerializeField] GameObject doorCollider;



    // Start is called before the first frame update
    void Start()
    {
        manager = Manager.instance;
    }

    void Update()
    {
        if(type == DoorType.Enemy)
        {
            checkForEnemies();
        }
        else if(type == DoorType.Trigger)
        {
            if(triggered == true)
            {
                openDoor();
            }
        }
    }

    private void checkForEnemies()
    {
        bool areAllDead = true;

        for(int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                areAllDead = false;
                break;
            }
        }

        if(areAllDead == true)
        {
            openDoor();
        }
    }


    private void openDoor()
    {
       Destroy(this.gameObject);
    }

    private void openBossDoor()
    {
       doorCollider.SetActive(false);

        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void closeBossDoor()
    {
        doorCollider.SetActive(true);

        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }




   private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            if(type == DoorType.Locked && manager.numberOfKeys > 0)
            {
                --manager.numberOfKeys;
                openDoor();
            }
            else if(type == DoorType.Boss && manager.hasBossKey == true)
            {
                manager.hasBossKey = false;
                openBossDoor();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            if(type == DoorType.Boss)
            {
                closeBossDoor();
            }
        }
    }
}
