using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonChaseState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;

    }

    public override void LogicUpdate()
    {
        if(currentEnemy.loseTimeCounter <= 0) // stop chasing after lose player for a while
        {
            currentEnemy.SwitchState(EnemyState.Patrol);
        }

        if(Physics2D.OverlapCircle(currentEnemy.attackPoint.position, currentEnemy.attackRadius, currentEnemy.attackerLayer))
        {
            // enter attack state
            currentEnemy.SwitchState(EnemyState.Attack);
        }

    }

    public override void PhysicsUpdate()
    {
        if(currentEnemy.isHurt)
            return;

        currentEnemy.anim.SetBool("isWalk", true);

        // chase player
        // follow player'x while keeping enemy itself's y
        if(currentEnemy.targetTrans != null)
        {
            currentEnemy.FlipTo(currentEnemy.targetTrans);

            currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, 
            new Vector2(currentEnemy.targetTrans.position.x, currentEnemy.transform.position.y), currentEnemy.currentSpeed * Time.fixedDeltaTime);
        }
        else
        {
            //Debug.Log("Follow Last Position");
            currentEnemy.FlipTo(currentEnemy.targetLastTrans);

            currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, 
            new Vector2(currentEnemy.targetLastTrans.position.x, currentEnemy.transform.position.y), currentEnemy.currentSpeed * Time.fixedDeltaTime);
        }
        
    }

    public override void OnExit()
    {
        currentEnemy.anim.SetBool("isWalk", false);
    }

}
