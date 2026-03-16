using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;
    PlayerController controls;

    Rigidbody2D rb;
    [SerializeField] BoxCollider2D feet;
    

    Vector2 move;
    public bool canMove = true;
    public bool facingRight = true;
    public float moveSpeed;
    [SerializeField] float oneItemMoveFactor;
    [SerializeField] float twoItemMoveFactor;

    public bool crouching = false;
    public float crawlSpeed;
    bool calledCrouchCheck = false;
    public bool canCrouch = true;
    [SerializeField] Collider2D clearanceChecker;

    public float jumpVelocity;
    [SerializeField] float airControl;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask platformLayerMask;

    [SerializeField] GameObject lightAttackSword;
    [SerializeField] GameObject heavyAttackSword;
    [SerializeField] GameObject idelSword;
    public bool swordDrawn;
    public bool canAttack = true;
    public bool attacking = false;
    [SerializeField] float lightAttackTime;
    [SerializeField] float heavyAttackTime;
    [SerializeField] int lightDamage;
    [SerializeField] int heavyDamage;



    [SerializeField] GameObject shield;
    [SerializeField] GameObject idleShield;
    [SerializeField] GameObject blockingShield;
    public bool shieldDrawn = false;
    [SerializeField] bool blocking = false;

    [SerializeField] float maxHP;
    [SerializeField] float currentHP;
    [SerializeField] List<GameObject> HPMarkers = new List<GameObject>();

    [SerializeField] bool moonwalking = false;
    [SerializeField] float moonwalkMoveFactor;

    [SerializeField] float defaultXKnockBackAmount;
    [SerializeField] float defaultYKnockBackAmount;
    float xKnockBackAmount;
    float yKnockBackAmount;
    float knockBackCounter;
    [SerializeField] float knockBackTotalTime;
    bool knockBackCheck = false;
    bool enemyIsToTheLeft;

    SpriteRenderer sprite;

    [SerializeField] bool onLadder = false;
    [SerializeField] bool climbingUp = false;
    [SerializeField] bool climbingDown = false;
    
    [SerializeField] float climbSpeed;



    /// <summary>
    /// the higher the value, the longer the player can jump after leaving the ground
    /// </summary>
    /// <returns></returns>
    [SerializeField] float coyoteTime = .2f;
    [SerializeField] float coyoteCounter;


    /// <summary>
    /// list of sounds the player makes to indicate that they took damage
    /// </summary>
    [SerializeField] public AudioSource[] damageSounds;


    [SerializeField] private AudioSource[] healthSounds;

    [SerializeField] GameObject pauseMenu;
    public bool paused = false;
   

    void Awake()
    {
        instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        

        xKnockBackAmount = defaultXKnockBackAmount;
        yKnockBackAmount = defaultYKnockBackAmount;


        controls = new PlayerController();
        controls.KeyboardControls.Enable();

        
        controls.KeyboardControls.Crouch.performed += ctx => crouch(true);
        controls.KeyboardControls.Stand.performed += ctx => crouch(false);

        controls.KeyboardControls.Jump.performed += ctx => jump();
        controls.KeyboardControls.StopJump.performed += ctx => stopJump();

        controls.KeyboardControls.DrawShield.performed += ctx => drawShield();
        controls.KeyboardControls.Block.performed += ctx => block(true);
        controls.KeyboardControls.StopBlock.performed += ctx => block(false);

        controls.KeyboardControls.Attack.performed += ctx => attack();
        controls.KeyboardControls.DrawSword.performed += ctx => drawSword();

        controls.KeyboardControls.Moonwalk.performed += ctx => moonwalk(true);
        controls.KeyboardControls.StopMoonwalk.performed += ctx => moonwalk(false);
        
        controls.KeyboardControls.EnableLook.performed += ctx => enableCameraMovement(true);
        controls.KeyboardControls.StopLook.performed += ctx => enableCameraMovement(false);

        controls.KeyboardControls.Pause.performed += ctx => pauseGame();

    }

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //shield display
        if(shieldDrawn == true)
        {
            if(attacking == false)
            {
                if(playerIsGrounded() == false)//in the air
                {
                    if(blocking == false)
                    {
                        shield.SetActive(true);
                        blockingShield.SetActive(false);
                        idleShield.SetActive(false);
                    }
                    else
                    {
                        shield.SetActive(false);
                        blockingShield.SetActive(true);
                        idleShield.SetActive(false);
                    }
                }
                else if(move.x == 0)// on the ground and not moving
                {
                    if(blocking == false)
                    {
                        shield.SetActive(true);
                        blockingShield.SetActive(false);
                        idleShield.SetActive(false);
                    }
                    else
                    {
                        shield.SetActive(false);
                        blockingShield.SetActive(true);
                        idleShield.SetActive(false);
                    }
                }
                else//on the ground and moving
                {
                    if(moonwalking == true)
                    {
                        if(blocking == false)
                        {
                            shield.SetActive(true);
                            blockingShield.SetActive(false);
                            idleShield.SetActive(false);
                        }
                        else
                        {
                            shield.SetActive(false);
                            blockingShield.SetActive(true);
                            idleShield.SetActive(false);
                        }
                    }
                    else
                    {
                        shield.SetActive(false);
                        blockingShield.SetActive(false);
                        idleShield.SetActive(true);
                    }
                }
            }
            else
            {
                shield.SetActive(false);
                blockingShield.SetActive(false);
                idleShield.SetActive(true);
            }            
        }
        else
        {
            shield.SetActive(false);
            blockingShield.SetActive(false);
            idleShield.SetActive(false);
        }
    
        handelSwordDisplay();

        clearanceChecker.enabled = true;
        clearanceChecker.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0);
    
        /*if(clearanceChecker.IsTouchingLayers(groundLayerMask) == true)
        {
            Debug.Log("CROUCH: touching ground");
        }
        else
        {
            Debug.Log("CROUCH: not touching ground");
        }*/



    


        if (playerIsGrounded() == true)
        {
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }
    }  

   
    void moonwalk(bool b)
    {
        if(b == true)
        {
            moonwalking = true;
        }
        else
        {
            moonwalking = false;
        }
    }

    void enableCameraMovement(bool b)
    {
        CameraController.instance.setMoveableCamera(b);
    }

    void pauseGame()
    {
        paused = !paused;

        if(paused == true)
        {
            Time.timeScale = 0;
            //canMove = false;
            //canCrouch = false;
            //canAttack = false;

            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            //canMove = true;
            //canCrouch = true;
            //canAttack = true;

            pauseMenu.SetActive(false);
        }
    }

#region Shield

    void drawShield()
    {
       if(paused == false)
        {
             shieldDrawn = !shieldDrawn;
        }
    }

    void block(bool b)
    {
        if(shieldDrawn == true && onLadder == false)
        {
            blocking = b;
        }
        else if(b == false)
        {
            blocking = false;
        }
        
        if(blocking == false && onLadder == true && crouching == false)
        {
            if(b == true)
            {
                climbingUp = true;
                climbingDown = false;
            }
            else
            {
                climbingUp = false;
            }
        }
    }

#endregion

#region Crouch

    void crouch(bool b)
    {
        if(paused == false)
        {
            if(playerIsGrounded() == true && climbingUp == false)
        {
            if(b == true && crouching == false && canAttack == true)
            {
                crouching = true;

                this.transform.localScale = new Vector3(1,1,1);
                this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y - .5f,0);

                shield.transform.localScale = new Vector3(0.2638f, 1, 1);
                shield.transform.localPosition = new Vector3(0.434f, 0, 0);

                blockingShield.transform.localScale = new Vector3(1, .25f, 1);

            }
            else
            {
                if(calledCrouchCheck == false)
                {
                    StartCoroutine(checkClearence());
                }
            }
        }
        else if(playerIsGrounded() == false && crouching == false && onLadder == true)
        {
            if(b == true)
            {
                climbingUp = false;
                climbingDown = true;
            }
            else
            {
                climbingDown = false;
            }
        }
        }
    }

    IEnumerator checkClearence()
    {
        calledCrouchCheck = true;

        /*RaycastHit2D h;

        do
        {
            h = Physics2D.BoxCast(new Vector3(this.transform.position.x, this.transform.position.y + 1), new Vector3(1,1,1),0f,Vector2.up,1f,groundLayerMask);

            //Debug.Log("CROUCH: h.collider = " + h.collider.gameObject.name);

            yield return new WaitForSeconds(0f);
        }
        while(h.collider != null);*/

        while(clearanceChecker.IsTouchingLayers(groundLayerMask) == true)
        {
            Debug.Log("CROUCH: checker is touching");
            yield return new WaitForSeconds(0f);
        }
        
        
        //stand up

        crouching = false;

        this.transform.localScale = new Vector3(1,2,1);
        this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y + .5f,0);

        shield.transform.localScale = new Vector3(0.2638f, 0.48444f, 1);
        shield.transform.localPosition = new Vector3(0.434f, .25f, 0);

        blockingShield.transform.localScale = new Vector3(1, .125f, 1);


        calledCrouchCheck = false;
    }

#endregion

#region Jump
  
    void jump()
    {
        if(playerIsGrounded() == true && coyoteCounter > 0 && crouching == false)
        {
            rb.velocity = Vector2.up * jumpVelocity;
        }
        else if(playerIsGrounded() == false && coyoteCounter > 0 && crouching == false)
        {
            float finalMoveSpeed;
            if(swordDrawn == true && shieldDrawn == true)
            {
                finalMoveSpeed = moveSpeed * twoItemMoveFactor;
            }
            else if (swordDrawn == true || shieldDrawn == true)
            {
                finalMoveSpeed = moveSpeed * oneItemMoveFactor;
            }
            else
            {
                finalMoveSpeed = moveSpeed;
            }

            if(moonwalking == true)
            {
                finalMoveSpeed  = finalMoveSpeed * moonwalkMoveFactor;
            }

            rb.velocity = new Vector2(move.x * finalMoveSpeed, jumpVelocity);
        }
        else if(playerIsGrounded() == false && onLadder == false && rb.velocity.y == 0)
        {
            rb.velocity = Vector2.up * jumpVelocity * .75f;
        }


        /*if(playerIsGrounded() == true && crouching == false)
        {
            rb.velocity = Vector2.up * jumpVelocity;
        }
        else if(playerIsGrounded() == false && onLadder == false && rb.velocity.y == 0)
        {
            rb.velocity = Vector2.up * jumpVelocity * .75f;
        }*/
    }

    public bool playerIsGrounded()
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

    public bool playerIsOnPlatform()
    {
        if(feet.IsTouchingLayers(platformLayerMask) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void stopJump()
    {
        //variable jump height
        if(rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * .5f);

            coyoteCounter = 0;
        }
    }
#endregion

#region Attack

    void attack()
    {
       if(canAttack == true && attacking == false && paused == false)
       {
            if(swordDrawn == true)
            {
                attacking = true;

                StartCoroutine(attackCoroutine());
            }
            else
            {
                drawSword();

                attacking = true;

                StartCoroutine(lightAttack());
            }
       }
    }

    IEnumerator attackCoroutine()
    {
        if(shieldDrawn == true)//light attack
        {
            canMove = false;

            idelSword.SetActive(false);
            lightAttackSword.SetActive(true);

            lightAttackSword.GetComponent<Collider2D>().enabled = true;

            yield return new WaitForSeconds(lightAttackTime);

            lightAttackSword.GetComponent<Collider2D>().enabled = false;

            idelSword.SetActive(true);
            lightAttackSword.SetActive(false);

            canMove = true;
        }
        else//heavy attack
        {
            if(playerIsGrounded() == true && crouching == false)
            {
                canMove = false;
                canCrouch = false;

                idelSword.SetActive(false);

                yield return new WaitForSeconds(.5f);

                heavyAttackSword.SetActive(true);

                heavyAttackSword.GetComponent<Collider2D>().enabled = true;

                yield return new WaitForSeconds(heavyAttackTime);

                heavyAttackSword.GetComponent<Collider2D>().enabled = false;

                idelSword.SetActive(true);
                heavyAttackSword.SetActive(false);

                canMove = true;
                canCrouch = true;
            }
            else
            {
                canMove = false;

                idelSword.SetActive(false);
                lightAttackSword.SetActive(true);

                rb.WakeUp();

                lightAttackSword.GetComponent<Collider2D>().enabled = true;

                yield return new WaitForSeconds(lightAttackTime);

                lightAttackSword.GetComponent<Collider2D>().enabled = false;

                idelSword.SetActive(true);
                lightAttackSword.SetActive(false);

                canMove = true;
            }
        }

        attacking = false;
    }

    IEnumerator lightAttack()
    {
        canMove = false;

            idelSword.SetActive(false);
            lightAttackSword.SetActive(true);

            lightAttackSword.GetComponent<Collider2D>().enabled = true;

            yield return new WaitForSeconds(lightAttackTime);

            lightAttackSword.GetComponent<Collider2D>().enabled = false;

            idelSword.SetActive(true);
            lightAttackSword.SetActive(false);

            canMove = true;

            attacking = false;
    }
    void drawSword()
    {
        if(paused == false)
        {
            swordDrawn = !swordDrawn;
        }
    }

    void handelSwordDisplay()
    {
        if(swordDrawn == true)
        {
            if(attacking == false)
            {
                idelSword.SetActive(true);
            }
            else
            {
                idelSword.SetActive(false);
            }
        }
        else
        {
            idelSword.SetActive(false);
        }
    }

#endregion    

#region Damage

    /// <summary>
    /// enemy does default knock back
    /// </summary>
    /// <param name="dmg"> the damage the enemy does</param>
    /// <param name="enemyXPos">the X coordinate of the enemy's position</param>
    public void takeDamage(float dmg, float enemyXPos)
    {
        currentHP -= dmg;

        playDamageSound();
        playHealthSound();

        if(currentHP <= 0)
        {
            Debug.Log("PLAYER IS DEAD");

            for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

            Time.timeScale = 0;

            canMove = false;
            canCrouch = false;
            canAttack = false;

            EndConditions.instance.gameOverScreen.SetActive(true);
        }

        StartCoroutine(damageColor());

        if(this.transform.position.x <= enemyXPos)
        {
            enemyIsToTheLeft = false;
        }
        else
        {
            enemyIsToTheLeft = true;
        }

        knockBackCounter = knockBackTotalTime;

        if(HPMarkers.Count > 0)
        {
            for(int i = 0; i < dmg; i++)
            {
                //Destroy(HPMarkers[0].gameObject);
                //HPMarkers.Remove(HPMarkers[0]);

                for(int j = 0; j < HPMarkers.Count; j++)
                {
                    if(HPMarkers[j].activeSelf == true)
                    {
                        HPMarkers[j].SetActive(false);
                        break;
                    }
                }
            }
        }
        
    }
    /// <summary>
    /// enemy does no knock back
    /// </summary>
    /// <param name="dmg">the damage the enemy does</param>
    public void takeDamage(float dmg)
    {
        currentHP -= dmg;

        playDamageSound();
        playHealthSound();

        if(currentHP <= 0)
        {
            Debug.Log("PLAYER IS DEAD");

            for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

            Time.timeScale = 0;

            canMove = false;
            canCrouch = false;
            canAttack = false;

            EndConditions.instance.gameOverScreen.SetActive(true);
        }

        StartCoroutine(damageColor());

        if(HPMarkers.Count > 0)
        {
            for(int i = 0; i < dmg; i++)
            {
                //Destroy(HPMarkers[0].gameObject);
                //HPMarkers.Remove(HPMarkers[0]);

                for(int j = 0; j < HPMarkers.Count; j++)
                {
                    if(HPMarkers[j].activeSelf == true)
                    {
                        HPMarkers[j].SetActive(false);
                        break;
                    }
                }
            }
        }
    }
    /// <summary>
    /// enemy does custom knock back
    /// </summary>
    /// <param name="dmg">the damage the enemy does</param>
    /// <param name="enemyXPos">the X coordinate of the enemy's position</param>
    /// <param name="knockBackXVel">the custom xKnockBackAmount</param>
    /// <param name="knockBackYVel">the custom yKnockBackAmount</param>
    public void takeDamage(float dmg, float enemyXPos, float xKBA, float yKBA)
    {
        currentHP -= dmg;

        playDamageSound();
        playHealthSound();

        if(currentHP <= 0)
        {
            Debug.Log("PLAYER IS DEAD");

            for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

            Time.timeScale = 0;

            canMove = false;
            canCrouch = false;
            canAttack = false;

            EndConditions.instance.gameOverScreen.SetActive(true);
        }

        StartCoroutine(damageColor());

        if(this.transform.position.x <= enemyXPos)
        {
            enemyIsToTheLeft = false;
        }
        else
        {
            enemyIsToTheLeft = true;
        }

        xKnockBackAmount = xKBA;
        xKnockBackAmount = yKBA;

        knockBackCounter = knockBackTotalTime;

        if(HPMarkers.Count > 0)
        {
            for(int i = 0; i < dmg; i++)
            {
                //Destroy(HPMarkers[0].gameObject);
                //HPMarkers.Remove(HPMarkers[0]);

                for(int j = 0; j < HPMarkers.Count; j++)
                {
                    if(HPMarkers[j].activeSelf == true)
                    {
                        HPMarkers[j].SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// plays random damage sound
    /// </summary>
    void playDamageSound()
    {
        for (int i = 0; i < damageSounds.Length; i++)
        {
            damageSounds[i].Stop();
        }
        int r = Random.Range(0, 3);
        damageSounds[r].Play();
    }

    /// <summary>
    /// plays sound based on currentHP
    /// </summary>
    void playHealthSound()
    {
        switch (currentHP)
        {
                case 10:
                case 9:
                    for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }
                    break;
                case 8:
                case 7:
                    for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

                    healthSounds[0].Play();
                    break;
                case 6:
                case 5:
                    for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

                    healthSounds[1].Play();
                    break;
                case 4:
                case 3:
                    for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

                    healthSounds[2].Play();
                    break;
                case 2:
                case 1:
                    for (int i = 0; i < healthSounds.Length; i++)
                    {
                        healthSounds[i].Stop();
                    }

                    healthSounds[3].Play();
                    break;
                default:
                    break;
        }
    }
    public void gainHP(float hp)
    {
        currentHP += hp;

        playHealthSound();

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        for(int i = 0; i < hp; i++)
        {
            for(int j = HPMarkers.Count - 1; j > -1; j--)
            {
                if(HPMarkers[j].activeSelf == false)
                {
                    HPMarkers[j].SetActive(true);
                    break;
                }
            }
        }
    }


    /// <summary>
    /// enemy does no damage and default knock back
    /// </summary>
    /// <param name="enemyXPos">the X coordinate of the enemy's position</param>
    public void knockBack(float enemyXPos)
    {
        if(this.transform.position.x <= enemyXPos)
        {
            enemyIsToTheLeft = false;
        }
        else
        {
            enemyIsToTheLeft = true;
        }

        knockBackCounter = knockBackTotalTime;
    }
    /// <summary>
    /// enemy does no damage and custom knock back
    /// </summary>
    /// <param name="enemyXPos">the X coordinate of the enemy's position</param>
    /// <param name="xVel">the custom xKnockBackAmount</param>
    /// <param name="yVel">the custom yKnockBackAmount</param>
    public void knockBack(float enemyXPos, float xKBA, float yKBA)
    {
        if(this.transform.position.x <= enemyXPos)
        {
            enemyIsToTheLeft = false;
        }
        else
        {
            enemyIsToTheLeft = true;
        }

        xKnockBackAmount = xKBA;
        xKnockBackAmount = yKBA;

        knockBackCounter = knockBackTotalTime;
    }


    IEnumerator damageColor()
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(.25f);

        sprite.color = Color.white;
    }

#endregion

    void FixedUpdate()
    {
        movementWithSomeAirControl();

        if(knockBackCounter > 0)
        {
            knockBackCheck = true;

            if(crouching == true && calledCrouchCheck == false)
            {
                StartCoroutine(checkClearence());
            }

            if(enemyIsToTheLeft == true)//if enemy is to the left, go right
            {
                rb.velocity = new Vector2(xKnockBackAmount,yKnockBackAmount);
            }
            else//if enemy is to the right, go left
            {
                rb.velocity = new Vector2(-xKnockBackAmount,yKnockBackAmount);
            }

            knockBackCounter -= Time.deltaTime;
        }

        if(knockBackCounter <= 0 && knockBackCheck == true && playerIsGrounded() == true)
        {
            xKnockBackAmount = defaultXKnockBackAmount;
            yKnockBackAmount = defaultYKnockBackAmount;

            rb.velocity = Vector2.zero;
            knockBackCheck = false;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ladder")
        {
            onLadder = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ladder")
        {
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Ladder")
        {
            onLadder = false;
            climbingUp = false;
            climbingDown = false;
        }
    }


#region Getters
    public int getLightDamage()
    {
        return lightDamage;
    }

    public int getHeavyDamage()
    {
        return heavyDamage;
    }

    public bool getBlocking()
    {
        return blocking;
    }

    public float getCurrentHP()
    {
        return currentHP;
    }

#endregion

#region Movement Types

    //not updated
    void movementWithFullAirControl()
    {
        if(canMove == true)
        {
            move = controls.KeyboardControls.Movement.ReadValue<Vector2>();
            
            if(move != new Vector2(0,0))
            {
                if(crouching == false)
                {
                    rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(move.x * crawlSpeed, rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0,0);
        }
    }

    //not updated
    void movementWithNoAirControl()
    {
        if(playerIsGrounded() == true)
        {
            if(canMove == true)
            {
                move = controls.KeyboardControls.Movement.ReadValue<Vector2>();
            
                if(move != new Vector2(0,0))
                {
                    if(crouching == false)
                    {
                        rb.velocity = new Vector2(move.x * moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(move.x * crawlSpeed, rb.velocity.y);
                    }
                }
                else
                {
                    rb.velocity = new Vector2(0,rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(0,0);
            }
        }
    }

    void movementWithSomeAirControl()
    {
        float finalMoveSpeed;
        if(swordDrawn == true && shieldDrawn == true)
        {
            finalMoveSpeed = moveSpeed * twoItemMoveFactor;
        }
        else if (swordDrawn == true || shieldDrawn == true)
        {
            finalMoveSpeed = moveSpeed * oneItemMoveFactor;
        }
        else
        {
            finalMoveSpeed = moveSpeed;
        }

        float finalCrawlSpeed;
        if(swordDrawn == true && shieldDrawn == true)
        {
            finalCrawlSpeed = crawlSpeed * twoItemMoveFactor;
        }
        else if (swordDrawn == true || shieldDrawn == true)
        {
            finalCrawlSpeed = crawlSpeed * oneItemMoveFactor;
        }
        else
        {
            finalCrawlSpeed = crawlSpeed;
        }

        if(moonwalking == true)
        {
            finalMoveSpeed  = finalMoveSpeed * moonwalkMoveFactor;
            finalCrawlSpeed  = finalCrawlSpeed * moonwalkMoveFactor;
        }
        
        
        if(canMove == true)
        {
            if(knockBackCounter <= 0)
            {
                move = controls.KeyboardControls.Movement.ReadValue<Vector2>();
            
                if(move != new Vector2(0,0))//player is moving
                {
                    //player direction
                    if(playerIsGrounded() == true && moonwalking == false)
                    {
                        if(move.x > 0)//facing right
                        {
                            this.transform.eulerAngles = Vector3.zero;
                            facingRight = true;
                        }
                        else//facing left
                        {
                            this.transform.eulerAngles = new Vector3(0,180,0);
                            facingRight = false;
                        }
                    }


                    if(crouching == false)
                    {
                        if(playerIsGrounded() == true)
                        {
                            rb.velocity = new Vector2(move.x * finalMoveSpeed, rb.velocity.y);
                        }
                        else
                        {
                            rb.velocity += new Vector2(move.x * finalMoveSpeed * airControl * Time.deltaTime, 0);
                            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -finalMoveSpeed, +finalMoveSpeed), rb.velocity.y);
                        }
                    }
                    else
                    {
                        rb.velocity = new Vector2(move.x * finalCrawlSpeed, rb.velocity.y);
                    }
                }
                else//player is not moving
                {
                    if(playerIsGrounded() == true && knockBackCounter <= 0 && playerIsOnPlatform() == false)
                    {
                        rb.velocity = new Vector2(0,rb.velocity.y);
                    }
                    else if(playerIsGrounded() == true && knockBackCounter <= 0 && playerIsOnPlatform() == true)
                    {
                        rb.velocity = new Vector2(rb.velocity.x,0);
                    }
                }


                if(climbingUp == true && climbingDown == false && onLadder == true)//climb up
                {
                    rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
                }
                else if(climbingDown == true  && climbingUp == false && onLadder == true)//climb down
                {
                    rb.velocity = new Vector2(rb.velocity.x, -climbSpeed);
                }
            }
        }
        else if(attacking == true)
        {
            if(playerIsGrounded() == true && knockBackCounter <= 0)
            {
                rb.velocity = new Vector2(0,0);
            }
            else
            {
                rb.velocity += new Vector2(move.x * finalMoveSpeed * airControl * Time.deltaTime, 0);
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -finalMoveSpeed, +finalMoveSpeed), rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0,0);
        }

        //terminal velocity when falling
        if(playerIsGrounded() == false)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-100,100));
        }
    }

#endregion

    


    void OnDrawGizmos()
    {
        //Gizmos.DrawCube(new Vector3(this.transform.position.x, this.transform.position.y + 1),new Vector3(1,1,1));
    }
}
