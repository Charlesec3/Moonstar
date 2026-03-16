using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzPlatform : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] bool moveRightFirst = false;
    [SerializeField] bool movingLeft;

    /// <summary>
    /// how far the platform moves to the left and to the right
    /// </summary>
    [SerializeField] float patrolDistance;
    float leftPatrolBound;
    float rightPatrolBound;    

    [SerializeField] float moveSpeed;

    [SerializeField] protected LayerMask groundLayerMask;

    [SerializeField] protected BoxCollider2D obstructionChecker;


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

    void FixedUpdate()
    {
        move();
    }



    void move()
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


    /// <summary>
    /// 
    /// </summary>
    /// <returns> retruns true if there is no obstruction</returns>
    protected bool checkForFloorAndObstruction()
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
}
