using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Bat : Enemy
{
    public Vector2 spawnPosition;
    public float chargeSpeed;
    public float detectionRange;
    public Transform player;
    public Vector2 playerPosition;
    public float timer;
    public float chargeCooldown;
    public float chargeTime;
    public bool isCharging;

    void Start()
    {
        spawnPosition = new Vector2(transform.position.x, transform.position.y);
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        timer = 0;
    }

    public override void Move()
    {
        // transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = FoundPlayer();
        if(ray)
        {
            // Debug.Log("Bee found player");
            player = ray.transform;
        }
            
        if(player == null)
            return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(isCharging)
        {
            if(timer <= chargeTime || Vector2.Distance(new Vector2(transform.position.x, transform.position.y), playerPosition) < 0.3f)
            {
                isCharging = false;
                rb.velocity = Vector2.zero;
            }
        }
        if(!isCharging){
            if(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), spawnPosition) > 0)
            {
                Vector2 direction = (spawnPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
                rb.velocity = direction * speed;
            }
            if(timer <= 0 && Vector2.Distance(transform.position, player.position) <= detectionRange) // if charge is not on cooldown
            {
                ColorFlash(Color.yellow);
                playerPosition = new Vector2(player.position.x, player.position.y);
                Invoke("Charge", 0.6f /* warning time */);
            }
        }
        
    }

    public void Charge()
    {
        Vector2 direction = (playerPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        rb.velocity = direction * chargeSpeed;
        isCharging = true;
        timer = chargeCooldown;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireCube((Vector2)transform.position + originOffset + new Vector2(detectDistance * math.sign(transform.localScale.x), 0), detectSize);
        // Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
