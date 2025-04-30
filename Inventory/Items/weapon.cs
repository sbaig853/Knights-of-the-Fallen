using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private PlayerStats playerStats;  // Reference to the PlayerStats component
    private bool playerAttacked = false;

    private void Start()
    {
        // Get the PlayerStats component from the player object
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats component not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if player is attacking and if collision is with an enemy
        if (PlayerAttack.isPlayerAttacking && !playerAttacked && collision.gameObject.TryGetComponent<enemyController>(out enemyController enemy))
        {

            Debug.Log("inside here1");
            // Get the player's attack damage from PlayerStats
            int attackDamage = playerStats.attackDamage;

            // Log the enemy's health before the attack
            Debug.Log($"{collision.name} Health before attack: {enemy.health}");

            // Log the attack damage
            Debug.Log($"Player attacking with {attackDamage} damage.");

         
            playerAttacked = true; // Prevent multiple attacks

            // Log the enemy's health after the attack
            Debug.Log($"{collision.name} Health after attack: {enemy.health}");
        }
    }

    private void Update()
    {
        // Reset the attack state when the player is no longer attacking
        if (!PlayerAttack.isPlayerAttacking)
        {
            playerAttacked = false;
        }
    }
}
