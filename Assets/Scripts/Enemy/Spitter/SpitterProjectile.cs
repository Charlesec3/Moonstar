using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterProjectile : ProjectileBase
{
    // Update is called once per frame
    void Update()
    {
        if(coll.IsTouchingLayers(player) == true)
        {
            if(Player.instance.getBlocking() == false)
            {
                Player.instance.takeDamage(damage);
                Debug.Log("SPITTER PROJECTILE: hit player");
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log("SPITTER PROJECTILE: hit player shield");
                Destroy(this.gameObject);
            }
        }
        else if(coll.IsTouchingLayers(collidable) == true)
        {
            Debug.Log("SPITTER PROJECTILE: hit ground");
            Destroy(this.gameObject);
        }
    }
}
