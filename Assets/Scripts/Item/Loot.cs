using UnityEngine;

public class Loot : MonoBehaviour
{
    public float disappearTime = 1f;
    public float invincibleTime = 1f;
    public ItemData item;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= invincibleTime)
        {
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
        if (timer >= disappearTime)
        {
            Destroy(gameObject);
        }
    }

    // When player pick up the item
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add it to inventory
            if(item.itemName == "Health")
            {
                InventoryManager.Instance.ItemFunctions("Health");
                Destroy(gameObject);
            }
            else if(item.itemName == "Gold")
            {
                InventoryManager.Instance.AddGold(Random.Range(1, 10));
                Destroy(gameObject);
            }
            else 
            {
                if(InventoryManager.Instance.AddItem(item))
                {
                    AudioManager.Instance.PlayMusic("pickup", false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
