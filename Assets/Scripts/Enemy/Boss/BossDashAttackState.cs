using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossDashAttackState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Boss entered dash attack state");
        currentEnemy = enemy;
        
        currentEnemy.currentSpeed = currentEnemy.dashSpeed;
        currentEnemy.isAttack = true;
        
        currentEnemy.canDash = true;
        currentEnemy.anim.SetTrigger("dashAttack");

        currentEnemy.attackTime = 2f;
        currentEnemy.attackTimeCounter = currentEnemy.attackTime;

    }
    public override void LogicUpdate()
    {
        if(currentEnemy.isAttack)
        {
            currentEnemy.attackTimeCounter -= Time.deltaTime;

            if(currentEnemy.attackTimeCounter <= 0) // attack ends
            {
                currentEnemy.SwitchState(EnemyState.Idle);
            }

            // RaycastHit2D targetRayCast = currentEnemy.FoundPlayer();
            
            // if(currentEnemy.attackTimeCounter <= 0 && targetRayCast) // within detect distance
            // {
            //     currentEnemy.targetTrans = targetRayCast.transform;
            //     currentEnemy.SwitchState(EnemyState.Chase);
            // }
            // else if(currentEnemy.attackTimeCounter <= 0 && !targetRayCast)
            // {
            //     currentEnemy.SwitchState(EnemyState.Wander);
            // }
        }

    }

    public override void PhysicsUpdate()
    {
        int faceDir = (int)math.sign(currentEnemy.transform.localScale.x);
        currentEnemy.rb.velocity = new Vector2(faceDir * currentEnemy.currentSpeed, currentEnemy.rb.velocity.y);
    }
    public override void OnExit()
    {
        currentEnemy.isAttack = false;
        currentEnemy.canDash = false;
        
        currentEnemy.rb.velocity = Vector2.zero;
    }
}
