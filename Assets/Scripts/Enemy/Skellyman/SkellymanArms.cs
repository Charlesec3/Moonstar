using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkellymanArms : MonoBehaviour
{
    [SerializeField] Skellyman skelly;

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
            Player.instance.knockBack(skelly.transform.position.x,skelly.getPlayerKnockBackX(), skelly.getPlayerKnockBackY());
            Debug.Log("SKELLY ARM: hit player shield");
        }
        else if(coll.IsTouchingLayers(player) == true)
        {
            coll.enabled = false;
            Player.instance.takeDamage(skelly.getDamage(),skelly.transform.position.x);
            Debug.Log("SKELLY ARM: hit player");
        }
    }
}
