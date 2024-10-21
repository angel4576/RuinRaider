using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<ItemData> merchandise;
    public ShopSlot[] shopSlots;

    void Start()
    {
        int index = 0;
        for (int i = 0; i < merchandise.Count; i++)
        {
            ItemData item = merchandise[i];
            if(index < shopSlots.Length)
                shopSlots[index].AddItem(item);
                index++;
        }
    }

    
}
