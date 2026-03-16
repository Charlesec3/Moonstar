using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeavyAttackSword : MonoBehaviour
{
    Player player;

    float damage;

    [SerializeField] float xKnockBack;
    [SerializeField] float yKnockBack;

    void Start()
    {
        player = Player.instance;

        damage = player.getHeavyDamage();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.GetComponent<EnemyBase>() == true)
        {
            EnemyBase e = other.GetComponent<EnemyBase>();

            if(e.subjectToHeavyKnockBack == true)
            {
                e.takeDamage(damage,xKnockBack,yKnockBack);
            }
            else
            {
                e.takeDamage(damage);
            }
        }
    }
}
