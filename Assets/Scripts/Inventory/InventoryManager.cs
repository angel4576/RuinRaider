using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> inventory;
    public int inventoryMaxSize;
    public int gold;
    protected PlayerStatus player;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        inventory = new List<ItemData>();
        player = gameObject.GetComponent<PlayerStatus>();
    }

    public bool AddItem(ItemData item)
    {   
        if(inventory.Count >= inventoryMaxSize)
        {
            Debug.Log("Inventory full!");
            return false;
        }
        Debug.Log(item.itemName);
        foreach(ItemData inventoryItem in inventory)
        {
            // already has this item 
            if(inventoryItem.itemName == item.itemName && inventoryItem.count > 0)
            {
                inventoryItem.count += 1;
                UIManager.Instance.SetItemCount(item);
                return true;
            }
        }
        // new item
        item.count = 1;
        UIManager.Instance.AddNewItem(item);
        inventory.Add(item);
        return true;
    }

    public bool UseItem(string itemName)
    {
        ItemData item = inventory.Find(item => item.itemName == itemName);
        if(item == null || item.count < 1)
        {
            Debug.Log("Does not have item: " + itemName);
            return false;
        }
        
        // update item count in backend
        item.count -= 1;
        if(item.count <= 0)
        {
            inventory.Remove(item);
        }
        UIManager.Instance.SetItemCount(item);

        ItemFunctions(itemName);
        return true;
    }

    public void AddGold(int num)
    {
        gold += num;
        if(gold < 0)
        {
            gold = 0;
        }
        UIManager.Instance.SetText("Gold", gold.ToString());
    }

    public void ItemFunctions(string itemName)
    {
        if(itemName == "Blue Potion") {
        }
        switch (itemName)
        {
            case "Blue Potion":
                player.SetSanity(50);
                break;
            case "Red Potion":
                player.HealPlayer(10);
                break;
            case "Health":
                player.SetSanity(10);
                break;
            default:
                Debug.Log(itemName + " has no effect");
                break;
        }
    }
}
