using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    Collider2D coll;
    [SerializeField] LayerMask collidable;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask playerShield;

    Rigidbody2D rb;

    [SerializeField] float fallSpeed;

    [SerializeField] bool playerDetected = false;

    [SerializeField] float damage;


    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        coll = this.gameObject.GetComponent<Collider2D>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(detectPlayer() == true && playerDetected == false)
        {
            playerDetected = true;

            rb.velocity = new Vector2(0,-fallSpeed);
        }

        if(playerDetected == true)
        {
            if(coll.IsTouchingLayers(playerShield) == true)
            {
                Destroy(this.gameObject);
            }
            else if(coll.IsTouchingLayers(player) == true)
            {
                Player.instance.takeDamage(damage);
                Destroy(this.gameObject);
            }
            else if(coll.IsTouchingLayers(collidable) == true)
            {
                Destroy(this.gameObject);
            }
        }
    }

    bool detectPlayer()
    {
        RaycastHit2D[] h;
        Debug.DrawRay(transform.position, Vector2.down, UnityEngine.Color.cyan);
        h = Physics2D.RaycastAll(transform.position, Vector2.down);

        if (h.Length != 0)
        {
            //Debug.Log("DIVER: h[0] = " + h[0].collider.gameObject.name);
            if (h[0].collider.gameObject.GetComponent<Player>() != null || h[0].collider.gameObject.tag == "Player Shield" || h[0].collider.gameObject.name == "Clearance Checker")
            {
                return true;
            }
        }

        return false;
    }
}
