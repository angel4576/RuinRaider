using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    private PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() 
    {
        
    }

    private void OnDisable() 
    {

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            // player.isHit = true; 
            Vector2 direction = (other.transform.position - transform.position).normalized;
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage, direction);
        }

        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerStatus>().TakeDamage((Vector2)this.gameObject.transform.position, damage);
        }

    }

}
