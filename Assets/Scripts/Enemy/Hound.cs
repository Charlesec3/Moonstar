using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hound : EnemyBase
{
   [SerializeField] bool moveRightFirst = false;

    [SerializeField] bool movingLeft;

    public bool canAttack = true;
    bool pouncing = false;
    bool canPounce = true;

    /// <summary>
    /// how far the enemy moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;

    [SerializeField] float moveSpeed;

    [SerializeField] float range;

    /// <summary>
    /// how long it takes to pounce
    /// </summary>
    [SerializeField] float pouncePrepTime;
    /// <summary>
    /// how long it takes before moving after pounce
    /// </summary>
    [SerializeField] float pounceCoolTime;
    /// <summary>
    /// how long between ponces
    /// </summary>
    [SerializeField] float pounceWait;

    [SerializeField] float pounceXVel;
    [SerializeField] float pounceYVel;



    void Awake()
    {
        setPatrolBounds();

        rb = this.gameObject.GetComponent<Rigidbody2D>();

        xKnockBackAmount = defaultXKnockBackAmount;
        yKnockBackAmount = defaultYKnockBackAmount;
    }

    // Start is called before the first frame update
    void Start()
    {
        movingLeft = moveRightFirst;
    }

    void Update()
    {
        if(pouncing == true)
        {
            if(isPlayerToTheLeft() == true)
            {
                this.transform.eulerAngles = Vector3.zero;
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0,180,0);
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

        if(knockBackCounter > 0)
        {
            knockBackCheck = true;

            if(isPlayerToTheLeft() == true)
            {
                rb.velocity = new Vector2(xKnockBackAmount,yKnockBackAmount);
            }
            else
            {
                rb.velocity = new Vector2(-xKnockBackAmount,yKnockBackAmount);
            }

            knockBackCounter -= Time.deltaTime;
        }

        if(knockBackCounter <= 0 && knockBackCheck == true && isGrounded() == true)
        {
            xKnockBackAmount = defaultXKnockBackAmount;
            yKnockBackAmount = defaultYKnockBackAmount;

            rb.velocity = Vector2.zero;
            knockBackCheck = false;
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
                    //this.transform.eulerAngles = Vector3.zero;

                    if(checkForFloorAndObstruction() == true && this.transform.position.x > leftPatrolBound)
                    {
                        rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        movingLeft = false;
                        this.transform.eulerAngles = new Vector3(0,180,0);
                    }
                }
                else
                {
                    //this.transform.eulerAngles = new Vector3(0,180,0);

                    if(checkForFloorAndObstruction() == true && this.transform.position.x < rightPatrolBound)
                    {
                        rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);
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

        if(playerInRange() == true && canAttack == true)
        {
            currentState = State.Attack;
        }
    }


    bool playerInRange()
    {
        if(Vector3.Distance(this.transform.position, Player.instance.transform.position) <= range)
        {
            if(movingLeft == true && isPlayerToTheLeft() == true)
            {
                return true;
            }
            else if(movingLeft == false && isPlayerToTheLeft() == false)
            {
                return true;
            }
        }

        return false;
    }

    bool checkForPlayer()
    {
        if(obstructionChecker.IsTouchingLayers(playerLayerMask) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void attackState()
    {
        //movement
        if(pouncing == false)
        {
            if(isPlayerToTheLeft() == true)
            {
                this.transform.eulerAngles = Vector3.zero;

                if(checkForFloorAndObstruction() == true && checkForPlayer() == false)
                {
                    rb.velocity = new Vector2(-1 * moveSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0,rb.velocity.y);
                }
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0,180,0);

                if(checkForFloorAndObstruction() == true && checkForPlayer() == false)
                {
                    rb.velocity = new Vector2(1 * moveSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0,rb.velocity.y);
                }
            }
        }



        if(pouncing == false && canPounce == true)
        {
            pouncing = true;
            StartCoroutine(pounce());
        }
    }


    IEnumerator pounce()
    {
        rb.velocity = new Vector2(0,0);

        yield return new WaitForSeconds(pouncePrepTime);

        if(isGrounded() == true)
        {
            if(isPlayerToTheLeft() == true)//jump left
            {
                rb.velocity = new Vector2(-pounceXVel,pounceYVel);
            }
            else//jump right
            {
                rb.velocity = new Vector2(pounceXVel,pounceYVel);
            }
        }

        canPounce = false;

        yield return new WaitForSeconds(pounceCoolTime);

        StartCoroutine(pounceTimer());

        pouncing = false;
    }

    IEnumerator pounceTimer()
    {
        yield return new WaitForSeconds(pounceWait);

        canPounce = true;
    }


    void setPatrolBounds()
    {
        leftPatrolBound = this.transform.position.x - patrolDistance;

        rightPatrolBound = this.transform.position.x + patrolDistance;
    }

    public override void setKnockBack()
    {
        knockBackCounter = knockBackTotalTime;
    }
    public override void setKnockBack(float xKBA, float yKBA)
    {
        xKnockBackAmount = xKBA;
        xKnockBackAmount = yKBA;

        knockBackCounter = knockBackTotalTime;
    }
}
