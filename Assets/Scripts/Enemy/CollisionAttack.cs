using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAttack : MonoBehaviour
{
    EnemyBase enemy;

    Collider2D coll;

    [SerializeField] LayerMask player;
    [SerializeField] LayerMask playerShield;

    bool timerRunning;

    [SerializeField] float timeBetweenAttacks;


    void Awake()
    {
        coll = this.gameObject.GetComponent<Collider2D>(); 
        enemy = this.transform.parent.gameObject.GetComponent<EnemyBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if(coll.IsTouchingLayers(player) == true && timerRunning == false)
        {
            Debug.Log("ROLLER: attack player");
            timerRunning = true;

            Player.instance.takeDamage(enemy.getDamage(),enemy.transform.position.x);

            StartCoroutine(attackTimer());
        }
    }

    IEnumerator attackTimer()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        timerRunning = false;

        this.gameObject.GetComponent<Collider2D>().enabled = true;
        this.transform.parent.gameObject.GetComponent<Rigidbody2D>().WakeUp();
    }
}
