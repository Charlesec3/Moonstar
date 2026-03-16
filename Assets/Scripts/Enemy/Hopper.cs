using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hopper : EnemyBase
{
    [SerializeField] bool moveRightFirst = false;
    [SerializeField] bool movingLeft;

    /// <summary>
    /// how far the enemy moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;    

    [SerializeField] float moveSpeed;

    public bool canJump = true;

    [SerializeField] float jumpVelocity;

    /// <summary>
    /// how long between jumps
    /// </summary>
    [SerializeField] float jumpTime;

    bool isJumpRunning = false;




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
        move();


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
                    if(checkForObstructions() == true && this.transform.position.x > leftPatrolBound)
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
                    if(checkForObstructions() == true && this.transform.position.x < rightPatrolBound)
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
            rb.velocity = new Vector2(0,rb.velocity.y);
        }

        if(canJump == true && isJumpRunning == false)
        {
            isJumpRunning = true;

            StartCoroutine(jump());
        }
    }

    bool checkForObstructions()
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


    IEnumerator jump()
    {
        yield return new WaitForSeconds(jumpTime);
        
        if(isGrounded() == true)
        {
            rb.velocity = Vector2.up * jumpVelocity;
        }

        isJumpRunning = false;
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
