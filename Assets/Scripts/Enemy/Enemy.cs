using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Speed")]
    public float speed;
    public float chaseSpeed;
    public float currentSpeed;

    [Header("Enemey Status")]
    public int health;
    public int damage;
    public bool isHurt;

    [Header("Hurt Setting")]
    public float hurtForce;
    public float hurtTime;
    public string onHitSFX = "enemy hit";

    [Header("Timer")]
    public float loseTimeCounter;
    public float loseTime;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public int patrolPosition;
    [HideInInspector]public Transform targetPoint;

    [Header("Detect Setting")]
    public float detectDistance;
    public float searchDistance; // larger range search
    public float rangeDistance;
    public Vector2 originOffset;
    public Vector2 detectSize;
    public Vector2 searchSize;
    public LayerMask attackerLayer;
    [HideInInspector]public Transform targetTrans;
    [HideInInspector]public Transform targetLastTrans;

    public Vector3 targetPos;

    [Header("Attack Setting")]
    public Transform attackPoint;
    public float attackRadius;
    public float attackTime;
    public float attackTimeCounter;
    public bool isAttack;
    public int attackIndex;
    public float attackCD;
    public float attackCDCounter;
    public bool canAttack;

    [Header("Wander Setting")]
    public float turnInterval; // change direction
    public float turnTimeCounter;
    public int wanderDirection;
    public float wanderTime;
    public float wanderTimeCounter;

    [Header("Chase Setting")]
    public float chaseTime;
    public float chaseTimeCounter;

    [Header("Dash Setting")]
    public float dashSpeed;
    public bool canDash;
    public float dashCoolDown;
    public float dashCDCounter;

    // Components
    private SpriteRenderer sr;
    [HideInInspector]public Animator anim;
    [HideInInspector]public Rigidbody2D rb;
    private Color color = Color.white;

    protected BaseState currentState;
    // States
    protected BaseState idleState;
    protected BaseState patrolState;
    protected BaseState chaseState;
    protected BaseState attackState;
    protected BaseState dashAttackState;
    protected BaseState wanderState;
    protected BaseState coolDownState;

    public VoidEventSO cameraShakeEvent;

    public virtual void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public abstract void Move();
    public virtual void PlayOnHitSFX() {
        AudioManager.Instance.PlayMusic(onHitSFX, false);
    }

    public virtual void Attack(){}

    public virtual void Die()
    {
        GetComponent<Lootbag>().spawnLoot(transform.position);
        Destroy(gameObject);
    }

    public virtual void TakeDamage(int damage, Vector2 direction) {
        health -= damage;

        isHurt = true;

        PlayOnHitSFX();
        ColorFlash(Color.red);
        
        anim.SetTrigger("hurt");
        rb.velocity = Vector2.zero; // Reset velocity to prevent accumulation
        StartCoroutine(Knockback(direction));

        if (health <= 0) {
            Die();
        }

        cameraShakeEvent.RaiseEvent();
    }

    public IEnumerator Knockback(Vector2 direction)
    {
        rb.AddForce(direction * hurtForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(hurtTime);

        isHurt = false;

    }

    public void ColorFlash(Color flashColor)
    {
        flashColor.a = 0.5f;
        sr.color = flashColor;
        Invoke("ResetColor", 0.2f /* flash time */);
    }

    public void ResetColor()
    {
        sr.color = color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // other.gameObject.GetComponent<PlayerStatus>().TakeDamage(this.transform.position);
        }
    }

    // FSM
    public void CountLoseTime()
    {
        // RaycastHit2D playerFound = FoundPlayer();
        if(!FoundPlayer() && loseTimeCounter > 0) // not found player
        {
            loseTimeCounter -= Time.deltaTime;
            
        }
        else if(FoundPlayer())
        {
            loseTimeCounter = loseTime;
            targetLastTrans = FoundPlayer().transform;
            // Debug.Log(targetLastTrans.position);
        }
    }


    public void FlipTo(Transform pointToGo)
    {
        if(pointToGo.position.x - transform.position.x > 0.1f) // target is on the right of enemy
        {
            // face right
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // face left
            transform.localScale = new Vector3(-1 * math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    // Detect
    public RaycastHit2D FoundPlayer()
    {
        RaycastHit2D targetRayCast = Physics2D.BoxCast((Vector2)transform.position + originOffset, detectSize, 0, new Vector2(math.sign(transform.localScale.x), 0), detectDistance, attackerLayer);
        // targetTrans = targetRayCast.transform;
        // targetPos = targetRayCast.transform.position;
        return targetRayCast;
    }

    public RaycastHit2D BiDirectionDetect()
    {
        RaycastHit2D targetRayCast1 = Physics2D.BoxCast((Vector2)transform.position + originOffset, searchSize, 0, new Vector2(math.sign(transform.localScale.x), 0), searchDistance, attackerLayer);
        RaycastHit2D targetRayCast2 = Physics2D.BoxCast((Vector2)transform.position + originOffset, searchSize, 0, new Vector2(math.sign(transform.localScale.x), 0), -searchDistance, attackerLayer);

        if(targetRayCast1)
            return targetRayCast1;
        else
            return targetRayCast2;

    }

    public RaycastHit2D RangeAttackDetect()
    {
        RaycastHit2D targetRayCast = Physics2D.BoxCast((Vector2)transform.position + originOffset, detectSize, 0, new Vector2(math.sign(transform.localScale.x), 0), rangeDistance, attackerLayer);
        
        return targetRayCast;
    }

    public void SwitchState(EnemyState state)
    {
        var newState = state switch 
        {
            EnemyState.Idle => idleState,
            EnemyState.Patrol => patrolState,
            EnemyState.Attack => attackState,
            EnemyState.Chase => chaseState,
            EnemyState.Wander => wanderState,
            EnemyState.DashAttack => dashAttackState,
            EnemyState.CoolDown => coolDownState,

            _ => null
        };

        currentState.OnExit(); // exit current state
        
        currentState = newState; // switch and enter
        currentState.OnEnter(this);
    }

    
}
