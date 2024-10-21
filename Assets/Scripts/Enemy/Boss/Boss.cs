using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : Enemy
{

    

    public override void Awake()
    {
        base.Awake();

        // initialize states
        idleState = new BossIdleState();
        chaseState  = new BossChaseState();
        attackState = new BossMeleeAttackState();
        wanderState = new BossWanderState();
        dashAttackState = new BossDashAttackState();
        coolDownState = new CoolDownState();

        // initally idle
        currentState = idleState;
        //currentState = wanderState;
    }

    private void OnEnable() 
    {
        currentState.OnEnter(this);
    }

    private void OnDisable() 
    {
        currentState.OnExit();
    }

    // Start is called before the first frame update
    void Start()
    {
        // dashCDCounter = dashCoolDown;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.LogicUpdate();

        // dash cd 
        if(!canDash)
        {
            dashCDCounter -= Time.deltaTime;
            if(dashCDCounter <= 0)
            {
                canDash = true;
                dashCDCounter = dashCoolDown;
            }
        }

        if(!canAttack)
        {
            attackCDCounter -= Time.deltaTime;
            if(attackCDCounter <= 0)
            {
                canAttack = true;
                attackCDCounter = attackCD;
            }
        }

        anim.SetBool("isAttack", isAttack);
        anim.SetInteger("attackIndex", attackIndex);
    }

    private void FixedUpdate() 
    {
        currentState.PhysicsUpdate();    
    }

    public void DashAttack()
    {
        rb.velocity = new Vector2();
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);

        Debug.Log(direction);
        if(direction.x > 0) // player is on the left of enemy
        {
            // enemy should face left
            transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if(direction.x < 0) // player is on the right of enemy
        {
            // enemy should face left
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        if(health <= 0)
        {
            // Display win scene
            UIManager.Instance.GetComponent<Transform>().Find("Win Scene").gameObject.SetActive(true);
        }

    }

    public override void Move()
    {
        
    }

    private void OnDrawGizmosSelected() 
    {
        // 双向detect
        Gizmos.DrawWireCube((Vector2)transform.position + originOffset + new Vector2(searchDistance * math.sign(transform.localScale.x), 0), searchSize);
        Gizmos.DrawWireCube((Vector2)transform.position + originOffset + new Vector2(-searchDistance * math.sign(transform.localScale.x), 0), searchSize);

        // found player detect
        Gizmos.DrawWireCube((Vector2)transform.position + originOffset + new Vector2(detectDistance * math.sign(transform.localScale.x), 0), detectSize);
        
        // range atk detect
        Gizmos.DrawCube((Vector2)transform.position + originOffset + new Vector2(rangeDistance * math.sign(transform.localScale.x), 0), detectSize);

        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
