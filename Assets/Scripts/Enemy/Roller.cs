using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : EnemyBase
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

    void shooterKilled(EnemyBase e)
    {
        Debug.Log( e.name + " killed");
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


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Player.instance.takeDamage(damage, this.transform.position.x);

            /*if(movingLeft == true)
            {
                movingLeft = false;
                this.transform.eulerAngles = new Vector3(0,180,0);
            }
            else
            {
                movingLeft = true;
                this.transform.eulerAngles = Vector3.zero;
            }*/
        }
    }
}
