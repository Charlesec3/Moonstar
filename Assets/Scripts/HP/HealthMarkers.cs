using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMarkers : MonoBehaviour
{
    [SerializeField] float maxHP;
    [SerializeField] float currentHP;
    [SerializeField] List<GameObject> HPMarkers = new List<GameObject>();

    [SerializeField] GameObject HPMarkersParrent;


    void OnEnable()
    {
        Player.onPlayerTakeDamage += removeHealthMarkers;
        Player.onPlayerGainHP += addHealthMarkers;
        Player.onGamePause += toggleDisplay;
    }

    void OnDisable()
    {
        Player.onPlayerTakeDamage -= removeHealthMarkers;
        Player.onPlayerGainHP -= addHealthMarkers;
        Player.onGamePause -= toggleDisplay;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void removeHealthMarkers(float dmg)
    {
        if(HPMarkers.Count > 0)
        {
            for(int i = 0; i < dmg; i++)
            {
                for(int j = 0; j < HPMarkers.Count; j++)
                {
                    if(HPMarkers[j].activeSelf == true)
                    {
                        HPMarkers[j].SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    void addHealthMarkers(float hp)
    {
        for(int i = 0; i < hp; i++)
        {
            for(int j = HPMarkers.Count - 1; j > -1; j--)
            {
                if(HPMarkers[j].activeSelf == false)
                {
                    HPMarkers[j].SetActive(true);
                    break;
                }
            }
        }
    }

    void toggleDisplay(bool isPaused)
    {
        if(isPaused == true)
        {
            HPMarkersParrent.SetActive(false);
        }
        else
        {
            HPMarkersParrent.SetActive(true);
        }
    }
}
