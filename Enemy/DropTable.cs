using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public ItemData item; // Reference to the item data
    public float dropChance; // Probability of dropping this item
    public GameObject itemPrefab;
}

public class DropTable : MonoBehaviour
{
    public List<DropEntry> drops = new List<DropEntry>(); // List of potential drops

    // Return DropEntry which contains both ItemData and itemPrefab
    public DropEntry GetRandomDrop()
    {
        // Calculate total drop chance and pick a random point
        float totalChance = 0f;
        foreach (var drop in drops) totalChance += drop.dropChance;

        float randomPoint = Random.value * totalChance;

        // Loop through the drops to find which one is selected based on the random point
        foreach (var drop in drops)
        {
            if (randomPoint < drop.dropChance)
            {
                Debug.Log($"Dropped item: {drop.item.itemName} with chance {drop.dropChance}");
                return drop; // Return the selected item
            }
            randomPoint -= drop.dropChance; // Decrease by the current drop's chance
        }

        return null; // No drop if nothing is selected
    }
}