using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy
{
    public override void Move()
    {
        
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        anim.SetTrigger("hurt");
        PlayOnHitSFX();

        cameraShakeEvent.RaiseEvent();
    }

    
}
