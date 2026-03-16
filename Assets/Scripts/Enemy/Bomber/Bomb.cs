using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float fuseTime;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject explosion;

    SpriteRenderer sprite;

    [SerializeField] float damage;

    float t = 0;


    void Awake()
    {
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(explode());
    }

    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.red,t);

        if(t < 1)
        {
            t += Time.deltaTime / fuseTime;
        }
    }


    IEnumerator explode()
    {
        yield return new WaitForSeconds(fuseTime);

        explosion = Instantiate(explosionPrefab,this.transform.position,this.transform.rotation);
        explosion.GetComponent<Explosion>().setDamage(damage);

        Destroy(this.gameObject);
    }
}
