using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{

    [SerializeField] private float activationDelay; 
    [SerializeField] private float activeTime; 

    [SerializeField] private float damageInterval = 1f; 

    [SerializeField] private int damage; 
    private Animator anim;
    private SpriteRenderer spriteRend;

    private Health playerHealth;

    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(ActiveFiretrap());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerHealth = null;
        }
    }

    private IEnumerator ActiveFiretrap()
    {
        while (true)
        {
            spriteRend.color = Color.red;

            //Trap is activated, turn on animation, and return color back to normal.
            yield return new WaitForSeconds(activationDelay);
            spriteRend.color = Color.white;
            active = true;
            anim.SetBool("activated", true);

            StartCoroutine(ApplyDamageOverTime());

            //Wait until x seconds, deactivate trap and reset to idle state. 
            yield return new WaitForSeconds(activeTime);
            active = false;
            triggered = false;
            anim.SetBool("activated", false);
            yield return new WaitForSeconds(activationDelay);
        }
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while(active)
        {
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            yield return new WaitForSeconds(damageInterval);
        }
    }
    
}
