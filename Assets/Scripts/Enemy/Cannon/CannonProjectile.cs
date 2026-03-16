using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : ProjectileBase
{
    [SerializeField] float playerKnockBackX;
    [SerializeField] float playerKnockBackY;


    // Update is called once per frame
    void Update()
    {
        if(coll.IsTouchingLayers(playerShield) == true)
        {
            Player.instance.knockBack(this.transform.position.x,playerKnockBackX, playerKnockBackY);
            Destroy(this.gameObject);
        }
        else if(coll.IsTouchingLayers(player) == true)
        {
            Player.instance.takeDamage(damage,this.transform.position.x,playerKnockBackX,playerKnockBackY);
            Destroy(this.gameObject);
        }
        else if(coll.IsTouchingLayers(collidable) == true)
        {
            Destroy(this.gameObject);
        }
    }
}
