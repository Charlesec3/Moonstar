using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected Collider2D coll;
    [SerializeField] protected LayerMask collidable;
    [SerializeField] protected LayerMask player;
    [SerializeField] protected LayerMask playerShield;
    [SerializeField] protected float damage;

    void Awake()
    {
        coll = this.gameObject.GetComponent<Collider2D>(); 
    }
    
}
