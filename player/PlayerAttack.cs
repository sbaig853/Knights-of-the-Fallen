using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static bool isPlayerAttacking = false;
    private float attackLength = 0.25f;
    private float timer = 0f;

    private Animator animator;
    private PlayerStats playerStats;

    public Collider2D attackingHitbox;
    
    private void Start()
    {
        attackingHitbox.enabled = false;
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>(); 
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L) && !isPlayerAttacking)
        {
            isPlayerAttacking = true;
            timer = attackLength;
            EnableHitbox();
        }



        if (isPlayerAttacking)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isPlayerAttacking = false;
                DisableHitbox();
                
            }
        }
    }

    public void EnableHitbox()
    {
        attackingHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        attackingHitbox.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerAttacking && collision.CompareTag("Enemy"))
        {
            // Handle the collision with the enemy
            Debug.Log("player attack");
            // Apply damage to the enemy
            enemyController enemy = collision.GetComponent<enemyController>();
            if (enemy != null)
            {
                int attackDamage = playerStats.attackDamage; // Get the attack damage from PlayerStats
                enemy.TakeDamage(attackDamage); // Use the attack value from PlayerStats
            }
        }
    }
}
