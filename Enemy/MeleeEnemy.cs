using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    

    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;
    [SerializeField] private Transform playerTransform;
    private playerMovement playerMovement;
    private bool playerRolling;



    private void Start()
    {
       playerMovement = FindObjectOfType<playerMovement>();
    }

    private void Awake() 
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        if (capsuleCollider == null) return;

        cooldownTimer += Time.deltaTime;

        FlipTowardsPlayer();

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    private bool PlayerInSight()
    {
        if (capsuleCollider == null) return false;

        // Scale the capsule size to account for the enemy's scaling
        Vector2 size = new Vector2(
            capsuleCollider.size.x * Mathf.Abs(transform.localScale.x),
            capsuleCollider.size.y * Mathf.Abs(transform.localScale.y)
        );

        // Adjust the offset to ensure alignment with the enemy's position
        Vector3 offset = transform.position + transform.right * colliderDistance * transform.localScale.x;

        // Perform the CapsuleCast
        RaycastHit2D hit = Physics2D.CapsuleCast(
            offset,                        // Starting position
            size,                          // Capsule size
            capsuleCollider.direction,     // Capsule direction (horizontal/vertical)
            0,                             // No rotation
            transform.right * transform.localScale.x, // Direction of the cast
            range,                         // Maximum range of the cast
            playerLayer                    // Layer to detect
        );

        // Debugging logs for detection
        

        // Check if the cast hit a player and assign the player health
        if (hit.collider != null)
        {
            
            playerHealth = hit.transform.GetComponent<Health>();
            return true;
        }

        return false;
    }

    private void FlipTowardsPlayer()
    {
        if(playerTransform == null) 
            return;
        
        if(playerTransform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (playerTransform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnDrawGizmos()
    {
        if (capsuleCollider != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(
                transform.position + transform.right * colliderDistance * transform.localScale.x,
                range
            );
        }
    }


    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            if (playerHealth != null)
            {
                playerRolling = playerMovement.isRolling;

                if (playerRolling == false)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
    }
}
