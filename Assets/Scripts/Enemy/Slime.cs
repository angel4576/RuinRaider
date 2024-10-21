using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public float moveSpeed = 0.5f;
    public float changeDirectionInterval = 5f;
    private float timer;
    private Vector2 direction = new Vector2(-1f, 0f);

    public override void Move()
    {
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= changeDirectionInterval)
        {
            direction.x = -direction.x;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            timer = 0f;
        }

        Move();
    }
}
