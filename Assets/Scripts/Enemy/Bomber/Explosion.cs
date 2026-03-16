using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float explosionTime;

    CircleCollider2D coll;
    Rigidbody2D rb;

    [SerializeField] LayerMask player;

    [SerializeField] float damage;


    void Awake()
    {
        coll = this.gameObject.GetComponent<CircleCollider2D>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("EXPLOSION: caused " + damage + " damage");
        
        /*if(this.gameObject.GetComponent<SpriteRenderer>().isVisible == true)
        {
            this.transform.GetComponent<AudioSource>().Play();
        }*/
        
        StartCoroutine(explosionTimer());
    }

    // Update is called once per frame
    void Update()
    {
        coll.enabled = true;
        rb.WakeUp();
    }

    IEnumerator explosionTimer()
    {
        yield return new WaitForSeconds(explosionTime);

        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("BOMB: hit player");
            Player.instance.takeDamage(damage,this.transform.position.x);
        }
    }

    public void setDamage(float dmg)
    {
        damage = dmg;
    }
}
