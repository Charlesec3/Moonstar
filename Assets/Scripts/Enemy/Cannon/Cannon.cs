using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : EnemyBase
{
    /// <summary>
    /// how often the enemy shoots
    /// </summary>
    [SerializeField] float reloadTime;

    public bool canAttack = true;

    [SerializeField] bool shooting = false;

    [SerializeField] GameObject projectilePrefab;
    GameObject projectile;

    [SerializeField] float projectileSpeed;

    [SerializeField] Transform bulletSpawn;

    [SerializeField] bool facingLeft = true;



    // Update is called once per frame
    void Update()
    {
        if(shooting == false && projectile == null && canAttack)
        {
            shooting = true;

            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        yield return new WaitForSeconds(reloadTime);

        projectile = Instantiate(projectilePrefab,bulletSpawn.position, bulletSpawn.rotation);

        if(facingLeft == true)
        {
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed,0);
        }
        else
        {
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed,0);
        }

        shooting = false;
    }

    public override void setKnockBack()
    {
        //throw new System.NotImplementedException();
    }
    public override void setKnockBack(float xKBA, float yKBA)
    {
        //throw new System.NotImplementedException();
    }


}
