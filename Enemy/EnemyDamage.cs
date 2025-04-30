using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private playerMovement playerMovement;
    private bool playerRolling;
    [SerializeField] protected int damage;

    private void Start()
    {
        // Find the player and get its playerMovement component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<playerMovement>();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        playerRolling = playerMovement.isRolling;
        if (collision.tag == "Player" && playerRolling == false)
           collision.GetComponent<Health>().TakeDamage(damage);
    }
}
