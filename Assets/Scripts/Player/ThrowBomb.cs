using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ThrowBomb : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform player;
    public Vector2 bombOffset;
    private Rigidbody2D bombRb;
    public Vector2 throwForce;

    private void Update() 
    {
        transform.localScale = player.localScale;
    }

    public void Throw()
    {
        bool hasBomb = GetComponent<InventoryManager>().UseItem("Bomb");
        if(hasBomb)
        {
            bombOffset.x *= math.sign(-transform.localScale.x);
            GameObject bomb = Instantiate(bombPrefab, (Vector2)transform.position + (bombOffset * new Vector2(math.sign(transform.localScale.x), 1)), transform.rotation);
            
            bombRb = bomb.GetComponent<Rigidbody2D>();

            bombRb.velocity = new Vector2(math.sign(transform.localScale.x) * throwForce.x, throwForce.y);
        } else {
            Debug.Log("No bomb!");
        }
    }

}
