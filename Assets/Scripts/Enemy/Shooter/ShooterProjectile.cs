using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterProjectile : ProjectileBase
{
    // Update is called once per frame
    void Update()
    {
        if(coll.IsTouchingLayers(playerShield) == true)
        {
            Debug.Log("SHOOTER PROJECTILE: hit player shield");
            Destroy(this.gameObject);
        }
        else if(coll.IsTouchingLayers(player) == true)
        {
            Player.instance.takeDamage(damage);
            Debug.Log("SHOOTER PROJECTILE: hit player");
            Destroy(this.gameObject);
        }
        else if(coll.IsTouchingLayers(collidable) == true)
        {
            Debug.Log("SHOOTER PROJECTILE: hit ground");
            Destroy(this.gameObject);
        }
    }
}
