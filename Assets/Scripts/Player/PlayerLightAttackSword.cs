using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightAttackSword : MonoBehaviour
{
    Player player;

    float damage;

    void Start()
    {
        player = Player.instance;

        damage = player.getLightDamage();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<EnemyBase>() == true)
        {
            EnemyBase e = other.GetComponent<EnemyBase>();

            if(e is Knight)
            {
                Knight k = (Knight)e;

                if(k.getBlockingHigh() == true && player.crouching == true)
                {
                    k.takeDamage(player.getLightDamage());
                }

                if(k.getBlockingHigh() == false && player.crouching == false)
                {
                    k.takeDamage(damage);
                }
            }
            else
            {
                e.takeDamage(damage);
            }
        }
    }
}
