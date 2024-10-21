using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Skeleton : Enemy
{
    // [Header("Patrol")]
    // public Transform[] patrolPoints;
    // public int patrolPosition;
    // [HideInInspector]public Transform targetPoint;

    // [Header("Detect Setting")]
    // public float detectDistance;
    // public Vector2 originOffset;
    // public Vector2 detectSize;
    // public LayerMask attackerLayer;
    // [HideInInspector]public Transform targetTrans;
    // [HideInInspector]public Transform targetLastTrans;

    

    // [Header("Attack Setting")]
    // public Transform attackPoint;
    // public float attackRadius;
    // public float attackTime;
    // public float attackTimeCounter;
    // public bool isAttack;

    public override void Awake() 
    {
        base.Awake();

        patrolState = new SkeletonPatrolState();
        chaseState = new SkeletonChaseState();
        attackState = new SkeletonAttackState();
        
        currentState = patrolState;
    }

    private void OnEnable() 
    {
        // currentSpeed = speed;

        // call onEnter when this is enabled
        currentState.OnEnter(this);
    }

    private void OnDisable() 
    {
        currentState.OnExit();    
    }

    // Update is called once per frame
    void Update()
    {
        CountLoseTime();

        currentState.LogicUpdate();

        anim.SetBool("isAttack", isAttack);
    }

    private void FixedUpdate() 
    {
        currentState.PhysicsUpdate();
    }

    public void MeleeAttack()
    {
        // anim.SetTrigger("attack");
    }

    #region Time Counter
    // public void CountLoseTime()
    // {
    //     // RaycastHit2D playerFound = FoundPlayer();
    //     if(!FoundPlayer() && loseTimeCounter > 0) // not found player
    //     {
    //         loseTimeCounter -= Time.deltaTime;
            
    //     }
    //     else if(FoundPlayer())
    //     {
    //         loseTimeCounter = loseTime;
    //         targetLastTrans = FoundPlayer().transform;
    //         // Debug.Log(targetLastTrans.position);
    //     }
    // }

    public void CountAttackTime()
    {
        // attackTimeCounter -= Time.deltaTime;
    }

    #endregion

    #region Called by State Machine 
    // public void FlipTo(Transform pointToGo)
    // {
    //     if(pointToGo.position.x - transform.position.x > 0.1f) // target is on the right of enemy
    //     {
    //         // face right
    //         transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    //     }
    //     else
    //     {
    //         // face left
    //         transform.localScale = new Vector3(-1 * math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    //     }
    // }

    // public RaycastHit2D FoundPlayer()
    // {
    //     RaycastHit2D targetRayCast = Physics2D.BoxCast((Vector2)transform.position + originOffset, detectSize, 0, new Vector2(math.sign(transform.localScale.x), 0), detectDistance, attackerLayer);
    //     targetTrans = targetRayCast.transform;
    //     return targetRayCast;
    // }

    // public void SwitchState(EnemyState state)
    // {
    //     var newState = state switch 
    //     {
    //         EnemyState.Patrol => patrolState,
    //         EnemyState.Attack => attackState,
    //         EnemyState.Chase => chaseState,
    //         _ => null
    //     };

    //     currentState.OnExit(); // exit current state
        
    //     currentState = newState; // switch and enter
    //     currentState.OnEnter(this);
    // }

    #endregion

   

    public override void Move()
    {
        // rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
       
    }

    public override void PlayOnHitSFX()
    {
        AudioManager.Instance.PlayMusic("skeleton", false);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireCube((Vector2)transform.position + originOffset + new Vector2(detectDistance * math.sign(transform.localScale.x), 0), detectSize);
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
    
}
