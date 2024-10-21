using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Lootbag : MonoBehaviour
{
    public GameObject ItemPrefab;
    public List<ItemData> ItemList = new List<ItemData>();
    public float dropForce = 5f;
    public int minDrop;
    public int maxDrop;
    // public int dropChance;

    List<ItemData> GetLoot()
    {
        int num = Random.Range(minDrop, maxDrop + 1);
        if(num == 0) {
            return null;
        }
        List<ItemData> drop = new List<ItemData>();
        for (int i = 0; i < num; i++)
        {
            int chance = Random.Range(0, 100);
            if(chance < 60)
            {
                drop.Add(ItemList[0]);
            }
            if(chance >= 60 && chance < 80)
            {
                drop.Add(ItemList[3]);
            }
            if(chance >= 80 && chance < 90)
            {
                drop.Add(ItemList[1]);
            }
            if(chance >= 90 && chance < 100)
            {
                drop.Add(ItemList[2]);
            }
        }
        return drop;
    }

    public void spawnLoot(Vector2 spawnPosition){
        List<ItemData> drop = GetLoot();
        if(drop != null)
        {
            foreach (ItemData itemData in drop)
            {
                GameObject ItemGameObject = Instantiate(ItemPrefab, spawnPosition, Quaternion.identity);
                ItemGameObject.GetComponent<SpriteRenderer>().sprite = itemData.icon;
                ItemGameObject.GetComponent<Loot>().item = itemData;

                Vector2 dropDirection = new Vector2(Random.Range(-5f, 5f), Random.Range(0f, 5f));
                ItemGameObject.GetComponent<Rigidbody2D>().AddForce(dropDirection * dropForce, ForceMode2D.Impulse);
            }
        }
    }
}
