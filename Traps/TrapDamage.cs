using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageInterval = 1f; 

    private Health playerHealth;
    private bool stayinTrap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
          playerHealth = collision.GetComponent<Health>();
          stayinTrap = true;

          StartCoroutine(ApplyDamageOverTime());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            stayinTrap = false;
            playerHealth = null;

            StopCoroutine(ApplyDamageOverTime());
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while(stayinTrap && playerHealth != null)
        {
            playerHealth.TakeDamage(damage);

            yield return new WaitForSeconds(damageInterval);
        }
    }
    
}

