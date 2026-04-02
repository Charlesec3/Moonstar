using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public enum State {Defend, Attack};
    public State currentState;

    protected Rigidbody2D rb;

    [SerializeField] protected float currentHP;

    [SerializeField] protected float damage;

    public bool canMove = true;

    [SerializeField] protected float defaultXKnockBackAmount;
    [SerializeField] protected float defaultYKnockBackAmount;
    protected float xKnockBackAmount;
    protected float yKnockBackAmount;

    protected float knockBackCounter;
    [SerializeField] protected float knockBackTotalTime;
    [SerializeField] protected bool knockBackCheck = false;

    [SerializeField] protected LayerMask groundLayerMask;
    [SerializeField] protected LayerMask playerLayerMask;

    [SerializeField] SpriteRenderer sprite;

    [SerializeField] protected BoxCollider2D floorChecker;
    [SerializeField] protected BoxCollider2D feet;
    [SerializeField] protected BoxCollider2D obstructionChecker;

    [SerializeField] GameObject healthPrefab;
    GameObject health;

    /// <summary>
    /// can the player's heavy attck change the enemy's knock back
    /// </summary>
    public bool subjectToHeavyKnockBack = true;

    public int enemyID;


    public void takeDamage(float dmg)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            QuestManager.instance.enemyDied(this);
            
            if(Random.Range(1,5) == 1)
            {
                health = Instantiate(healthPrefab,this.transform.position,this.transform.rotation);
            }
            
            Destroy(this.gameObject);
        }


        if(currentState != State.Attack)
        {
            currentState = State.Attack;
        }

        StartCoroutine(damageColor());

        setKnockBack();
    }

    public void takeDamage(float dmg, float xKnockBack, float yKnockBack)
    {
        currentHP -= dmg;

        if(currentHP <= 0)
        {
            if(Random.Range(1,6) == 1)
            {
                health = Instantiate(healthPrefab,this.transform.position,this.transform.rotation);
            }
            
            Destroy(this.gameObject);
        }


        if(currentState != State.Attack)
        {
            currentState = State.Attack;
        }

        StartCoroutine(damageColor());

        setKnockBack(xKnockBack, yKnockBack);
    }


    IEnumerator damageColor()
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(.25f);

        sprite.color = Color.blue;
    }

    protected bool isPlayerToTheLeft()
    {
        if(Player.instance.transform.position.x <= this.transform.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    protected bool isGrounded()
    {
        if(feet.IsTouchingLayers(groundLayerMask) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns> retruns true if there is no obstruction</returns>
    protected bool checkForFloorAndObstruction()
    {
        if(floorChecker.IsTouchingLayers(groundLayerMask) == true && obstructionChecker.IsTouchingLayers(groundLayerMask) == false && knockBackCounter <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public abstract void setKnockBack();
    public abstract void setKnockBack(float KBA, float yKBA);
    
    

    public float getDamage()
    {
        return damage;
    }

    public float getXKnockBackAmount()
    {
        return xKnockBackAmount;
    }

    public void setXKnockBackAmount(float f)
    {
        xKnockBackAmount = f;
    }

    public float getYKnockBackAmount()
    {
        return yKnockBackAmount;
    }

    public void setYKnockBackAmount(float f)
    {
        yKnockBackAmount = f;
    }

}
