using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : EnemyBase
{
    /// <summary>
    /// how often the enemy spits
    /// </summary>
    [SerializeField] float[] reloadTimes;

    public bool canSpit = true;
    [SerializeField] bool spitting = false;

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject projectile;

    [SerializeField] float projectileSpeed;

    [SerializeField] bool moveRightFirst = false;
    [SerializeField] bool movingLeft;

    /// <summary>
    /// how far the enemy moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;    

    [SerializeField] float moveSpeed;

    [SerializeField] Transform bulletSpawn;


    void Awake()
    {
        setPatrolBounds();

        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        movingLeft = moveRightFirst;
    }

    void Update()
    {
        if(spitting == false && projectile == null && canSpit == true)
        {
            spitting = true;

            StartCoroutine(spit());
        }

    }

    void FixedUpdate()
    {
        move();
    }


    void move()
    {
        if(canMove == true)
        {
            if(knockBackCounter <= 0)
            {
                if(movingLeft == true)
                {
                    if(checkForObstruction() == true && this.transform.position.x > leftPatrolBound)
                    {
                        rb.velocity = new Vector2(-1 * moveSpeed, 0);
                    }
                    else
                    {
                        movingLeft = false;
                        this.transform.eulerAngles = new Vector3(0,180,0);
                    }
                }
                else
                {
                    if(checkForObstruction() == true && this.transform.position.x < rightPatrolBound)
                    {
                        rb.velocity = new Vector2(1 * moveSpeed, 0);
                    }
                    else
                    {
                        movingLeft = true;
                        this.transform.eulerAngles = Vector3.zero;
                    }
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(0,0);
        }
    }

    void setPatrolBounds()
    {
        leftPatrolBound = this.transform.position.x - patrolDistance;

        rightPatrolBound = this.transform.position.x + patrolDistance;
    }

    IEnumerator spit()
    {
        float waitTime = reloadTimes[Random.Range(0,reloadTimes.Length)];
        yield return new WaitForSeconds(waitTime);

        projectile = Instantiate(projectilePrefab,bulletSpawn.position, bulletSpawn.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-projectileSpeed);

        spitting = false;
    }


/// <summary>
/// 
/// </summary>
/// <returns> retruns true if there is no obstruction</returns>
    protected bool checkForObstruction()
    {
        if(obstructionChecker.IsTouchingLayers(groundLayerMask) == false)
        {
            return true;
        }
        else
        {
            return false;
        }
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
