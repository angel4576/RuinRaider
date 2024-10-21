using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    private PlayerInputControl inputActions;
    private PlayerAnimation playerAnimation;
    private PlayerStatus status;
    private PhysicsCheck physicsCheck;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    public int faceDirection;
    
    [Header("Event Listener")]
    public SceneLoadEventSO loadEvent;
    public VoidEventSO afterSceneLoadEvent;

    [Header("Control Setting")]
    private Vector2 inputDirection;
    public float speed;
    public float slideSpeed;
    public float speedDivisor;
    public float gravityDivisor;
    public float rbGravityScale;

    [Header("Attack Setting")]
    public bool isAttack;
    public float airAttackForce;
    // public bool isHit;

    [Header("Jump Setting")]
    [HideInInspector]public float jumpForce;
    public float initialVelocityY;
    public float gravity;
    public int airJumpCount;
    public bool isJump;


    [Header("Dash Setting")]
    public float dashCoolDown;
    public float dashCoolDownCounter;
    public float dashSpeed;
    public float dashTime;
    public float dashTimeCounter;
    public float slideTime;

    [Header("Dash Status")]
    public bool isDash; // start dash
    public bool isDashing;
    public bool canDash;

    [Header("Physics Materials")]
    public PhysicsMaterial2D wall;
    public PhysicsMaterial2D chara;

    [Header("Player Status")]
    public bool isHurt;
    public float hurtForce;
    public bool isDead;

    [Header("Bomb")]
    public GameObject bomb;
    public Vector3 bombOffset;
    private ThrowBomb throwBomb;

    private void Awake() 
    {
        // initialize
        inputActions = new PlayerInputControl();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        
        playerAnimation = GetComponent<PlayerAnimation>();
        status = GetComponent<PlayerStatus>();
        throwBomb = GetComponentInChildren<ThrowBomb>();


        // binding behaviors
        // inputActions.Gameplay.Jump.started += Jump;
        inputActions.Gameplay.Attack.started += MeleeAttack;
        inputActions.Gameplay.HeavyAttack.started += HeavyAttack;

        inputActions.Gameplay.Dash.started += Dash;
        inputActions.Gameplay.Bomb.started += ThrowBomb;

        inputActions.Gameplay.Jump.started += NewJump;

        inputActions.Enable();

    }

    private void OnEnable() 
    {
        //inputActions.Enable();

        loadEvent.loadRequestEvent += OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised += OnAfterSceneLoadEvent;
    }

    private void OnDisable() 
    {
        inputActions.Disable();

        loadEvent.loadRequestEvent -= OnLoadEvent;
        afterSceneLoadEvent.OnEventRaised -= OnAfterSceneLoadEvent;

    }

    


    #region Update
    // Update is called once per frame
    // Change mark in Update, change physics in fixedUpdate
    void Update()
    {
        // read input system MOVE value
        inputDirection = inputActions.Gameplay.Move.ReadValue<Vector2>();

        // dash cool down count
        if(!canDash)
        {
            dashCoolDownCounter -= Time.deltaTime;
            if(dashCoolDownCounter <= 0)
            {
                canDash = true;  
            }
        }

        // dash time count
        // dash includes dashing & sliding
        if(isDash)
        {
            dashTimeCounter -= Time.deltaTime;
            if(dashTimeCounter <= 0)
            {
                // dash end
                isDashing = false;

                // can't move for a very short time after dashing 
                if(dashTimeCounter <= -slideTime) // dash ends, sliding ends
                {
                    isDash = false;
                }
            }           
        }

        // reset double jump
        if(physicsCheck.isLeftOnGround || physicsCheck.isRightOnGround)
        {
            airJumpCount = 1;
        }

        // kill player when press "0" for debugging
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Die();
        }

        SwitchMaterial();
        
    }

    private void FixedUpdate() 
    {
        if(!isAttack && !isDash && !isHurt)
            Move();

        Dashing();

        // jump update
        if(isJump)
        {
            UpdateVelocityY();
        }
    }

    #endregion

    // disable control when load scene
    private void OnLoadEvent(GameSceneSO arg0, Vector3 arg1, bool arg2)
    {
        inputActions.Gameplay.Disable();
    }

    // enable control when load scene complete
    private void OnAfterSceneLoadEvent()
    {
        inputActions.Gameplay.Enable();
    }

    #region Input Control
    private void Move()
    {
        rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);

        FlipDirection();
    }

    // private void Jump(InputAction.CallbackContext context)
    // {
    //     if(physicsCheck.isLeftOnGround || physicsCheck.isRightOnGround)
    //     {
    //         airJumpCount = 1;
    //         rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    //         // jumpCount++;
    //     }
    //     else // in the air
    //     {
    //         if(airJumpCount == 1) // can jump 1 more time in the air
    //         {
    //             airJumpCount = 0;
    //             rb.velocity = new Vector2(rb.velocity.x, 0);
    //             rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    //         }
    //     }
            
        
    // }

    private void NewJump(InputAction.CallbackContext context)
    {
        if(isAttack)
            return;
        
        AudioManager.Instance.PlayMusic("jump", false);

        if(physicsCheck.isTouchForward)
            rb.velocity = new Vector2(0, rb.velocity.y);

        if(physicsCheck.isLeftOnGround || physicsCheck.isRightOnGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, initialVelocityY); // initial speed
            // airJumpCount = 1;
        }
        else // in the air
        {
            if(airJumpCount == 1) // can jump 1 more time in the air
            {
                rb.velocity = new Vector2(rb.velocity.x, initialVelocityY);
                // AudioManager.Instance.PlayMusic("jump", false);

                airJumpCount = 0;
            }
        }
        
    }

    private void UpdateVelocityY()
    {
        if(physicsCheck.isTouchForward)
            rb.velocity = new Vector2(0, rb.velocity.y);
            
        // V = V0 - gt
        float velocityY = rb.velocity.y;
        velocityY -= gravity * Time.fixedDeltaTime; // update y velocity based on formula
        rb.velocity = new Vector2(rb.velocity.x, velocityY);

        // when land on ground
        if((physicsCheck.isLeftOnGround || physicsCheck.isRightOnGround) && rb.velocity.y <= 0)
        {
            isJump = false;
        }
    }

    private void MeleeAttack(InputAction.CallbackContext context)
    {
        isAttack = true; 
        // rb.velocity = new Vector2(faceDirection * slideSpeed, rb.velocity.y);
        playerAnimation.PlayAttack();

        if(physicsCheck.isLeftOnGround && physicsCheck.isRightOnGround) // on the ground
            rb.velocity = new Vector2(0, rb.velocity.y);

        if(!isAttack) {
            AudioManager.Instance.PlayMusic("attack1", false);
        }

        // air attack
        if(!isAttack && !physicsCheck.isLeftOnGround && !physicsCheck.isRightOnGround)
        {
            // rb.AddForce(new Vector2(0, airAttackForce), ForceMode2D.Impulse);
            rb.gravityScale /= gravityDivisor;
        }
    }

    private void HeavyAttack(InputAction.CallbackContext context)
    {
        isAttack = true;

        playerAnimation.PlayHeavyAttack();

        // 下劈
        if(!isAttack && !physicsCheck.isLeftOnGround && !physicsCheck.isRightOnGround)
        {
            Debug.Log("下劈");
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, airAttackForce), ForceMode2D.Impulse);

            status.TriggerInvulnerability();
        }
    }

    private void ThrowBomb(InputAction.CallbackContext context)
    {
        // if bomb num >= 1
        if(!isAttack && !isHurt)
            throwBomb.Throw();

    }

    #endregion

    public void GetHurt(Vector2 attacker)
    {
        isHurt = true;
        // knock back
        rb.velocity = Vector2.zero;
        Vector2 knockDir = new Vector2(transform.position.x - attacker.x, 0).normalized;
        rb.AddForce(hurtForce * knockDir, ForceMode2D.Impulse);

        playerAnimation.PlayHurt();
    }

    public void Die()
    {
        isDead = true;
        inputActions.Gameplay.Disable();
    }

    #region Dash
    private void Dash(InputAction.CallbackContext context)
    {   
        // cool down
        if(dashCoolDownCounter <= 0)
        {
            canDash = true;
            dashCoolDownCounter = dashCoolDown; // set counter to count down
        }
            
        if(canDash && !isAttack)
            TriggerDash();
    }

    private void Dashing()
    {
        if(isDashing)
        {
            rb.velocity = new Vector2(faceDirection * dashSpeed, 0);
        }
        else if(!isDashing && isDash)// dashing ends, start sliding
        {
            rb.velocity = new Vector2(faceDirection * speed / speedDivisor, rb.velocity.y);
            rb.gravityScale = rbGravityScale; // recover gravity
        }
    }

    private void TriggerDash()
    {
        isDash = true;
        isDashing = true;

        // start counting dash time
        dashTimeCounter = dashTime;

        // cannot dash for a while after a dash
        canDash = false;  
        
        // dash 
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0; // prevent falling

        playerAnimation.PlayDash();    

        status.TriggerInvulnerability();
        
    }

    #endregion

    private void SwitchMaterial()
    {
        if(!physicsCheck.isLeftOnGround && !physicsCheck.isRightOnGround) // in the air
        {
            coll.sharedMaterial = wall;
        }
        else
        {
            coll.sharedMaterial = chara;
        }
    }

    private void FlipDirection()
    {
        faceDirection = (int)transform.localScale.x;
        
        if(inputDirection.x > 0) 
            faceDirection = 1;  // facing right
        if(inputDirection.x < 0)
            faceDirection = -1;
        
        transform.localScale = new Vector3(faceDirection * math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

    }
}
