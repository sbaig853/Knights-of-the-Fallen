using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // A list to store the player's items (excluding consumables)
    public List<ItemData> items = new List<ItemData>();

    // Adds an item to the inventory (only weapons and non-consumables)
    public void AddItem(ItemData item)
    {
        // Only add weapons or other valid items (not consumables)
        if (item != null && (item.type == ItemData.Type.Sword || item.type == ItemData.Type.Axe))
        {
            items.Add(item);
            Debug.Log($"Added {item.itemName} to inventory.");
        }
        else
        {
            Debug.Log($"Cannot add {item.itemName} to inventory (not a valid weapon).");
        }
    }

   

    // Get the first weapon item of a specific type
    public ItemData GetWeaponItem(string weaponName)
    {
        foreach (ItemData item in items)
        {
            if ((item.type == ItemData.Type.Sword || item.type == ItemData.Type.Axe) && item.itemName == weaponName)
            {
                return item; // Return the weapon ItemData
            }
        }
        return null; // If no weapon is found, return null
    }
}
