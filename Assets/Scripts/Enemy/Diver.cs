using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diver : EnemyBase
{
    public bool canAttack;

    [SerializeField] bool moveRightFirst = false;
    [SerializeField] bool movingLeft;

    /// <summary>
    /// how far the enemy moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;    

    [SerializeField] float moveSpeed;
    [SerializeField] float diveSpeed;

    [SerializeField] float range;

    [SerializeField] bool playerDetected = false;

    [SerializeField] bool isTracer;
    [SerializeField] Vector3 dirToPlayer;

    Collider2D coll;
    [SerializeField] LayerMask collidable;
    [SerializeField] LayerMask player;
    [SerializeField] LayerMask playerShield;


    void Awake()
    {
        setPatrolBounds();

        rb = this.gameObject.GetComponent<Rigidbody2D>();
        coll = this.gameObject.GetComponent<Collider2D>(); 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        movingLeft = moveRightFirst;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerDetected == false)
        {
            if(detectPlayer() == false)
            {
                currentState = State.Defend;
            }
            else
            {
                playerDetected = true;

                if(isTracer == false)
                {
                    dirToPlayer = Player.instance.transform.position - this.transform.position;
                }

                currentState = State.Attack;
            }
        }

        if(playerDetected == true && isTracer == true)
        {
            dirToPlayer = Player.instance.transform.position - this.transform.position;
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
    
    void FixedUpdate()
    {
        switch(currentState)
        {
            case State.Defend:
                move();
                break;
            case State.Attack:
                if(canAttack == true)
                {
                    attackState();
                }
                break;
        }
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


    void attackState()
    {
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.position += transform.up * diveSpeed * Time.deltaTime;
    }


    bool detectPlayer()
    {
        RaycastHit2D[] h;
        Vector3 localDirToPlayer = Player.instance.transform.position - transform.position;
        Debug.DrawRay(transform.position, localDirToPlayer, UnityEngine.Color.cyan);
        h = Physics2D.RaycastAll(transform.position, localDirToPlayer,range);

        if (h.Length != 0)
        {
            //Debug.Log("DIVER: h[0] = " + h[0].collider.gameObject.name);
            if (h[0].collider.gameObject.GetComponent<Player>() != null || h[0].collider.gameObject.tag == "Player Shield")
            {
                //Debug.Log("DIVER: ray hit");

                //Debug.Log("DIVER: is isPlayerToTheLeft() = " + isPlayerToTheLeft());

                if(movingLeft == true && isPlayerToTheLeft() == true)
                { 
                    //Debug.Log("DIVER: player detected facing the right way");
                    return true;
                }
                else if(movingLeft == false && isPlayerToTheLeft() == false)
                {
                    return true;
                }
            }
        }
        else
        {
            //Debug.Log("DIVER: h.Length = 0");
        }

        return false;
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

    void setPatrolBounds()
    {
        leftPatrolBound = this.transform.position.x - patrolDistance;

        rightPatrolBound = this.transform.position.x + patrolDistance;
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
