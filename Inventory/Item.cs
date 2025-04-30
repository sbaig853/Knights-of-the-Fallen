using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData; // Reference to the item data
    private bool isPickedUp = false; // Flag to check if the item has been picked up

    // This method sets up the item data and applies the sprite icon
    public void Setup(ItemData data)
    {
        itemData = data;
        if (itemData != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && itemData.icon != null)
            {
                spriteRenderer.sprite = itemData.icon;
            }
        }
    }

}