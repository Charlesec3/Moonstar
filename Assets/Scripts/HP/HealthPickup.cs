using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() == true)
        {
            Player.instance.gainHP(2);
            Destroy(this.gameObject);
        }
    }
}
