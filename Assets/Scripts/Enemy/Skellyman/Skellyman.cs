using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skellyman : EnemyBase
{
    [SerializeField] bool moveRightFirst = false;

    [SerializeField] bool movingLeft;

    public bool canAttack = true;
    bool attackRunning = false;

    /// <summary>
    /// how far the enemy moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;

    [SerializeField] float moveSpeed;

    [SerializeField] float range;

    [SerializeField] float attackTime;
    [SerializeField] float timeBetweenAttacks;

    [SerializeField] GameObject idleArm1;
    [SerializeField] GameObject attackArm1;
    [SerializeField] GameObject arm2;

    [SerializeField] float playerKnockBackX;
    [SerializeField] float playerKnockBackY;


    
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


    void setPatrolBounds()
    {
        leftPatrolBound = this.transform.position.x - patrolDistance;

        rightPatrolBound = this.transform.position.x + patrolDistance;
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

    void attackState()
    {
        //movement
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
            
        if(attackRunning == false)
        {
            attackRunning = true;
            StartCoroutine(attack());
        }
    }

    IEnumerator attack()
    {        
        idleArm1.SetActive(false);
        attackArm1.SetActive(true);
        attackArm1.GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(.25f);

        idleArm1.SetActive(true);
        attackArm1.SetActive(false);

        yield return new WaitForSeconds(timeBetweenAttacks);

        arm2.SetActive(true);
        arm2.GetComponent<Collider2D>().enabled = true;

        yield return new WaitForSeconds(.25f);

        arm2.SetActive(false);

        yield return new WaitForSeconds(attackTime);

        attackRunning = false;
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


    public float getPlayerKnockBackX()
    {
        return playerKnockBackX;
    }

    public float getPlayerKnockBackY()
    {
        return playerKnockBackY;
    }
}
