using System.Collections;
using System.Collections.Generic;
using System.Net.Cache;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Door[] doors;
   

   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() != null)
        {
            for(int i = 0; i < doors.Length; i++)
            {
                doors[i].triggered = true;
            }

            Destroy(this.gameObject);
        }
    }
}
