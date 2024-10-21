using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : Enemy
{
    public SpriteRenderer sr;
    public Sprite brokenSprite;
    private bool isBroken;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void Move(){}

    public override void TakeDamage(int damage, Vector2 direction)
    {
        if(!isBroken)
        {
            Die();
        }
    }

    public override void Die()
    {
        PlayOnHitSFX();
        GetComponent<Lootbag>().spawnLoot(transform.position);
        sr.sprite = brokenSprite;
        isBroken = true;
    }
}
