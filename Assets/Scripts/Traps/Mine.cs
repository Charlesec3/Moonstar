using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    GameObject explosion;

    [SerializeField] bool timed;
    [SerializeField] float fuseTime;

    [SerializeField] float damage;

    SpriteRenderer sprite;

    float t = 0;
    bool calledExplode = false;


    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if(timed == true && calledExplode == true)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.red,t);

            if(t < 1)
            {
                t += Time.deltaTime / fuseTime;
            }
        }
    }

    IEnumerator explode()
    {
        calledExplode = true;
        
        yield return new WaitForSeconds(fuseTime);

        explosion = Instantiate(explosionPrefab,this.transform.position,this.transform.rotation);
        explosion.GetComponent<Explosion>().setDamage(damage);


        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Player>() == true || other.gameObject.GetComponent<Icicle>() == true)
        {
            if(timed == false)
            {
                explosion = Instantiate(explosionPrefab,this.transform.position,this.transform.rotation);
                explosion.GetComponent<Explosion>().setDamage(damage);

                Destroy(this.gameObject);
            }
            else
            {
                sprite.color = Color.red;
                StartCoroutine(explode());
            }
        }
    }
}
