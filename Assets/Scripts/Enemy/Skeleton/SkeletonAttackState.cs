using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        // Debug.Log("Enter Attack State");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;

        currentEnemy.attackTimeCounter = currentEnemy.attackTime;
        currentEnemy.isAttack = true;

        currentEnemy.anim.SetTrigger("attack");        
    }

    public override void LogicUpdate()
    {
        // play animation
        // currentEnemy.anim.SetBool("isAttack", currentEnemy.isAttack);
        // currentEnemy.anim.SetTrigger("attack");

        // if(!currentEnemy.isAttack)
        // {
        //     currentEnemy.SwitchState(EnemyState.Chase);
        // }

        if(currentEnemy.isAttack)
        {
            currentEnemy.attackTimeCounter -= Time.deltaTime;
            if(currentEnemy.attackTimeCounter <= 0 && currentEnemy.FoundPlayer())
            {
                currentEnemy.SwitchState(EnemyState.Chase);
            }
            else if(currentEnemy.attackTimeCounter <= 0 && !currentEnemy.FoundPlayer())
            {
                currentEnemy.SwitchState(EnemyState.Patrol);
            }
        }

    }

    public override void PhysicsUpdate()
    {

    }
    public override void OnExit()
    {   
        // Debug.Log("exit attack");
        currentEnemy.isAttack = false;
    }
}
