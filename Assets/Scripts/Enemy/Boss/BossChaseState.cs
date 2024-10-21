using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Enter Chase");
        currentEnemy = enemy;

        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;

        currentEnemy.chaseTimeCounter = currentEnemy.chaseTime;
    }

    public override void LogicUpdate()
    {
        // player in attack range
        if(Physics2D.OverlapCircle(currentEnemy.attackPoint.position, currentEnemy.attackRadius, currentEnemy.attackerLayer))
        {
            int atkProb = Random.Range(0, 100);
            if(atkProb < 2)
            {
                if(currentEnemy.canAttack)
                    currentEnemy.SwitchState(EnemyState.Attack);
            }
            else
            {
                // currentEnemy.SwitchState(EnemyState.Idle);
            }
                
        }

        // if player in a certain distance -> may or may not dash attack 
        if(currentEnemy.RangeAttackDetect()) // within range atk distance 
        {
            int dashProb = Random.Range(0, 100);

            if(dashProb < 5) // no use ?
            {
                if(currentEnemy.canDash)
                {
                    // currentEnemy.canDash = true;
                    currentEnemy.SwitchState(EnemyState.DashAttack);
                }
            }
            
        }


        // Count time
        currentEnemy.chaseTimeCounter -= Time.deltaTime; // potential bug
        if(currentEnemy.chaseTimeCounter <= 0)
        {
            currentEnemy.SwitchState(EnemyState.Wander);
        }
    }

    public override void PhysicsUpdate()
    {
        if(currentEnemy.isHurt)
            return;

        currentEnemy.anim.SetBool("isWalk", true);

        if(currentEnemy.targetTrans != null)
        {
            currentEnemy.FlipTo(currentEnemy.targetTrans);

            // chase to player position 
            currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, 
            new Vector2(currentEnemy.targetTrans.position.x, currentEnemy.transform.position.y), currentEnemy.currentSpeed * Time.fixedDeltaTime);
        }
        // currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, 
        // new Vector2(currentEnemy.targetPos.x, currentEnemy.transform.position.y), currentEnemy.currentSpeed * Time.fixedDeltaTime);
    
    }

    public override void OnExit()
    {
        Debug.Log("Exit Boss Chase");
        currentEnemy.anim.SetBool("isWalk", false);

    }
   
}
