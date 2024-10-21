using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeleeAttackState : BaseState
{
    
    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Boss entered attack state");
        currentEnemy = enemy;
        currentEnemy.currentSpeed = 0;
        
        currentEnemy.canAttack = true;
        currentEnemy.isAttack = true;
        
        currentEnemy.anim.SetTrigger("attack");

        // pick 1 attack way
        int atkI = Random.Range(0, 2);
        currentEnemy.attackTime = atkI switch
        {
            0 => 0.9f,
            1 => 2.7f,
            _ => 0f,
        };

        currentEnemy.attackIndex = atkI;
        currentEnemy.attackTimeCounter = currentEnemy.attackTime;

    }

    public override void LogicUpdate()
    {
        if(currentEnemy.isAttack)
        {
            currentEnemy.attackTimeCounter -= Time.deltaTime;
            RaycastHit2D targetRayCast = currentEnemy.FoundPlayer();
            if(currentEnemy.attackTimeCounter <= 0 && targetRayCast) // within detect distance
            {
                currentEnemy.targetTrans = targetRayCast.transform;
                currentEnemy.SwitchState(EnemyState.Chase);
            }
            else if(currentEnemy.attackTimeCounter <= 0 && !targetRayCast)
            {
                currentEnemy.SwitchState(EnemyState.Wander);
            }
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        Debug.Log("Exit Boss Attack");
        currentEnemy.isAttack = false;
        currentEnemy.canAttack = false;
    }

    
}
