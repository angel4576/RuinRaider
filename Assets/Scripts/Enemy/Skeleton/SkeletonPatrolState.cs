using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        // get current enemy when enter a state
        currentEnemy = enemy;

        currentEnemy.currentSpeed = currentEnemy.speed;

        currentEnemy.patrolPosition = 1;
        currentEnemy.targetPoint = currentEnemy.patrolPoints[currentEnemy.patrolPosition];
    }

    public override void LogicUpdate()
    {
        RaycastHit2D targetRayCast = currentEnemy.FoundPlayer();
        //if(currentEnemy.FoundPlayer())
        if(targetRayCast)
        {
            currentEnemy.targetTrans = targetRayCast.transform;
            // enter chase state
            currentEnemy.SwitchState(EnemyState.Chase);
        }
    }

    public override void PhysicsUpdate()
    {
        if(currentEnemy.isHurt)
            return;

        currentEnemy.anim.SetBool("isWalk", true);

        currentEnemy.FlipTo(currentEnemy.targetPoint);

        currentEnemy.transform.position = Vector2.MoveTowards(currentEnemy.transform.position, 
        new Vector2(currentEnemy.targetPoint.position.x, currentEnemy.transform.position.y), currentEnemy.currentSpeed * Time.fixedDeltaTime);
    
        // check the distance between enemy & current patrol point
        // only care the x-coord of the patrol points
        Vector2 enemyPosX = new Vector2(currentEnemy.transform.position.x, 0);
        Vector2 targetPointPosX = new Vector2(currentEnemy.targetPoint.position.x, 0);
        if(Vector2.Distance(enemyPosX, targetPointPosX) < 0.1f)
        {
            // update patrol point
            if(currentEnemy.patrolPosition == 0)
            {
                currentEnemy.patrolPosition = 1;
            }  
            else
            {
                currentEnemy.patrolPosition = 0;
            }
                
            currentEnemy.targetPoint = currentEnemy.patrolPoints[currentEnemy.patrolPosition];
        }
    }

    public override void OnExit()
    {
        // Debug.Log("exit patrol");
        currentEnemy.anim.SetBool("isWalk", false);
    }

}
