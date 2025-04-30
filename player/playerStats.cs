using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int attackDamage = 5;  // Starting attack damage
    public int defense = 0;
    public bool hasWeapon = false;
    public bool hasAxe = false;

    // Method to apply the weapon buff to the player
    public void ApplyWeaponBuff(int attackBuff)
    {
        attackDamage = attackBuff;  // Add the buff to the player's current attack damage
        Debug.Log("Player Attack Damage after applying weapon buff: " + attackDamage);
    }

    public void Defend(int blockAmount)
    {
        defense += blockAmount;
        Debug.Log("Player Defense Increased: " + defense);
    }

    public void RemoveDefense(int blockAmount)
    {
        defense -= blockAmount;
        Debug.Log("Player Defense Decreased: " + defense);
    }
}
