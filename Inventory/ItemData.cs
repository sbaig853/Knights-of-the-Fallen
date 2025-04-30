using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public int healthRestoreAmount = 0;
    public int attackBuffAmount = 0;
    public Sprite icon;
    public GameObject itemPrefab;
    public enum Type { Default, Consumable, Sword, Axe, Ammunition, Shield }
    public Type type = Type.Default;
}
