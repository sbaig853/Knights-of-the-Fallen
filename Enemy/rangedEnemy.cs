using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangedEnemy : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private float patrolSpeed;
    private Transform currentTarget;

    [Header("Fireball Settings")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private Transform playerTransform;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private bool isPatrolling = true;

   
    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentTarget = leftEdge; // Start patrolling towards the left edge
    }

    private void Update()
    {
        if (capsuleCollider == null) return;

        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            isPatrolling = false;
            FlipTowardsPlayer();

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangedAttack");
            }
        }
        else
        {
            isPatrolling = true;
        }

        if (isPatrolling)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (leftEdge == null || rightEdge == null) return;

        // Move towards the current target
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, patrolSpeed * Time.deltaTime);

        // Check if the enemy reached the current target
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            // Switch target to the other edge
            currentTarget = currentTarget == leftEdge ? rightEdge : leftEdge;
        }

        // Adjust facing direction
        if (currentTarget.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (currentTarget.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        int fireballIndex = FindFireball();

        fireballs[fireballIndex].transform.position = firepoint.position;
        fireballs[fireballIndex].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    private bool PlayerInSight() {
    if (capsuleCollider == null) return false;

    Vector2 size = new Vector2(capsuleCollider.size.x * range, capsuleCollider.size.y);

    Vector2 forwardDirection = transform.right * transform.localScale.x;
    Vector2 backwardDirection = -transform.right * transform.localScale.x;

    RaycastHit2D forwardHit = Physics2D.CapsuleCast(
        transform.position + (Vector3)(forwardDirection * colliderDistance),
        size,
        capsuleCollider.direction,
        0,
        forwardDirection,
        range,
        playerLayer
    );

    RaycastHit2D backwardHit = Physics2D.CapsuleCast(
        transform.position + (Vector3)(backwardDirection * colliderDistance),
        size,
        capsuleCollider.direction,
        0,
        backwardDirection,
        range,
        playerLayer
    );

    if (forwardHit.collider != null)
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        return true;
    }
    else if (backwardHit.collider != null)
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        return true;
    }

    return false;
}

    private void FlipTowardsPlayer()
    {
        if (playerTransform == null) return;

        if (playerTransform.position.x > transform.position.x && transform.localScale.x < 0)
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

        // Draw patrol edges
        Gizmos.color = Color.blue;
        if (leftEdge != null)
        {
            Gizmos.DrawSphere(leftEdge.position, 0.1f);
        }
        if (rightEdge != null)
        {
            Gizmos.DrawSphere(rightEdge.position, 0.1f);
        }
    }
}
