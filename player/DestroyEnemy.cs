/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    private PlayerStats playerStats; // Reference to the player's stats

    private void Start()
    {
        // Try to find the PlayerStats component
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            Debug.Log("Player Attack Damage: " + playerStats.attackDamage);
        }
        else
        {
        }
    }
  


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy") && PlayerAttack.isPlayerAttacking)
        {
            Debug.Log("Enemy hit by stomp!");
            Debug.Log("Player Attack Damage (including weapon bonus): " + playerStats.attackDamage);

            var enemy = other.gameObject.GetComponent<enemyController>();
                enemy.TakeDamage(1);
                PlayerAttack.isPlayerAttacking=false;

            }
        }
}
*/