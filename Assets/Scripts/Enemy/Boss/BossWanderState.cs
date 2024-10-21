using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossWanderState : BaseState
{

    public override void OnEnter(Enemy enemy)
    {
        Debug.Log("Enter Boss Wander");
        currentEnemy = enemy;

        currentEnemy.currentSpeed = currentEnemy.speed;

        // intialize turn setting
        currentEnemy.wanderDirection = (int)math.sign(currentEnemy.transform.localScale.x);
        currentEnemy.turnTimeCounter = currentEnemy.turnInterval;
    
        // initialize wander time setting
        currentEnemy.wanderTimeCounter = currentEnemy.wanderTime;
    }
    public override void LogicUpdate()
    {
        currentEnemy.anim.SetBool("isWalk", true);
        // Random turn behavior
        currentEnemy.turnTimeCounter -= Time.deltaTime;
        if(currentEnemy.turnTimeCounter <= 0)
        {
            // change direction
            currentEnemy.wanderDirection *= -1;
            currentEnemy.transform.localScale = new Vector2(currentEnemy.wanderDirection * math.abs(currentEnemy.transform.localScale.x), currentEnemy.transform.localScale.y);

            // reset timer
            currentEnemy.turnInterval = UnityEngine.Random.Range(1, 4);
            currentEnemy.turnTimeCounter = currentEnemy.turnInterval;        
        }

        // Count wander state time
        currentEnemy.wanderTimeCounter -= Time.deltaTime;
        if(currentEnemy.wanderTimeCounter <= 0)
        {
            // assign player transform to boss
            currentEnemy.targetTrans = currentEnemy.BiDirectionDetect().transform;
            currentEnemy.SwitchState(EnemyState.Chase);
        }

        // Enter attack state if player is within attack range
        if(Physics2D.OverlapCircle(currentEnemy.attackPoint.position, currentEnemy.attackRadius, currentEnemy.attackerLayer))
        {
            // Debug.Log("Player Enter Attack Range");
            currentEnemy.SwitchState(EnemyState.Attack);
        }
    }

    public override void PhysicsUpdate()
    {
        currentEnemy.rb.velocity = new Vector2(currentEnemy.wanderDirection * currentEnemy.currentSpeed, currentEnemy.rb.velocity.y);
    }
    public override void OnExit()
    {
        Debug.Log("Exit Wander State");
        currentEnemy.anim.SetBool("isWalk", false);
    }

    

}
