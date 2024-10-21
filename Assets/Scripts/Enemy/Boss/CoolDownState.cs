using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;

        currentEnemy.dashCDCounter = currentEnemy.dashCoolDown;
    }
    public override void LogicUpdate()
    {
        if(currentEnemy.canDash == false)
        {
            currentEnemy.dashCDCounter -= Time.deltaTime;
            if(currentEnemy.dashCDCounter <= 0) // cd ends
            {
                currentEnemy.canDash = true;
                currentEnemy.SwitchState(EnemyState.Idle);
            }
        }
        
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
