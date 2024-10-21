using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer rend;
    public Sprite openSprite;
    public Sprite closedSprite;
    public bool isDone; // interactable status

    private void Awake() 
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() 
    {
        // check interactable status when the object becomes active
        rend.sprite = isDone ? openSprite : closedSprite;

    }
    public void TriggerAction()
    {
        if(!isDone)
        {
            bool hasKey = InventoryManager.Instance.UseItem("Key");
            if(hasKey){
                OpenChest();
            } else {
                Debug.Log("need key!");
            }
        }
        
    }

    private void OpenChest()
    {
        AudioManager.Instance.PlayMusic("discover", false);
        rend.sprite = openSprite;
        isDone = true;
        this.gameObject.tag = "Untagged";
        GetComponent<Lootbag>().spawnLoot(transform.position);
    }

}
