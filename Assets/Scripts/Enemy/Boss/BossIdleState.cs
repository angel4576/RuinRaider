using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BaseState
{
    private int behaviorI;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        currentEnemy.currentSpeed = 0;

        behaviorI = Random.Range(0, 1);
        behaviorI = 1;
    }

    public override void LogicUpdate()
    {
        if(behaviorI == 0)
        {
            RaycastHit2D targetRayCast = currentEnemy.FoundPlayer();
            if(targetRayCast) // within detect distance
            {
                // Debug.Log("Boss Found Player");
                currentEnemy.targetTrans = targetRayCast.transform;
                currentEnemy.SwitchState(EnemyState.Chase);
            }
        }
        else if(behaviorI == 1)
        {
            currentEnemy.SwitchState(EnemyState.Wander);
        }
        
        
    }

    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        
    }
}
