using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSword : MonoBehaviour
{
    [SerializeField] Knight knight;

    Collider2D coll;

    [SerializeField] protected LayerMask player;
    [SerializeField] protected LayerMask playerShield;
    

    void Awake()
    {
        coll = this.gameObject.GetComponent<Collider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(coll.IsTouchingLayers(playerShield) == true)
        {
            coll.enabled = false;
            Debug.Log("Knight Sword: hit player shield");
        }
        else if(coll.IsTouchingLayers(player) == true)
        {
            coll.enabled = false;
            Player.instance.takeDamage(knight.getDamage(),knight.transform.position.x);
            Debug.Log("Knight Sword: hit player");
        }
    }
}
