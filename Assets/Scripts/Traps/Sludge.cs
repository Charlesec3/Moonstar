using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    /// <summary>
    /// how much damage the sludge does
    /// </summary>
    public int sludgeDmg;

    /// <summary>
    /// the amount of time between when the sludge does damage
    /// </summary>
    [SerializeField] private float waitTime;

    /// <summary>
    /// checks if waiting is done
    /// </summary>
    [SerializeField] private bool waitCheck;

    [SerializeField] private float moveSpeedFactor;
    [SerializeField] private float jumpVelocityFactor;

    bool playerInSludge = false;

    Coroutine c;



    void Update()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;    
    }

    IEnumerator damageOverTime()
    {
        waitCheck = true;

        yield return new WaitForSeconds(waitTime);

        if(playerInSludge == true)
        {
            Player.instance.takeDamage(sludgeDmg);
        }

        waitCheck = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() == true)
        {
            playerInSludge = true;

            Debug.Log("SLUDGE: player enter");

            Player p = other.gameObject.GetComponent<Player>();

            p.moveSpeed = p.moveSpeed / moveSpeedFactor;
            p.crawlSpeed = p.crawlSpeed / moveSpeedFactor;
            p.jumpVelocity = p.jumpVelocity / jumpVelocityFactor;

            p.takeDamage(sludgeDmg);

            waitCheck = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() == true)
        {
            Debug.Log("SLUDGE: player stay");

            if(waitCheck == false)
            {
                c = StartCoroutine(damageOverTime());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() == true)
        {
            playerInSludge = false;

            if(c != null)
            {
                StopCoroutine(c);
            }

            Player p = other.gameObject.GetComponent<Player>();

            p.moveSpeed = p.moveSpeed * moveSpeedFactor;
            p.crawlSpeed = p.crawlSpeed * moveSpeedFactor;
            p.jumpVelocity = p.jumpVelocity * jumpVelocityFactor;
        }
    }
}
