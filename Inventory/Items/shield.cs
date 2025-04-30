using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public int blockAmount = 5;  // Amount of damage blocked by the shield
    public float cooldownTime = 3f;  // Cooldown time before the shield can block again
    public float duration = 5f;  // Duration the shield is active for
    private bool isShieldActive = false;
    private bool isOnCooldown = false;
    private PlayerStats playerStats;

    private void Start()
    {
        // Get PlayerStats when the shield is created
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Method to activate the shield
    public void ActivateShield()
    {
        if (isOnCooldown || isShieldActive) return;

        isShieldActive = true;
        playerStats.Defend(blockAmount); // Apply defense (you may need to add this method in PlayerStats)
        StartCoroutine(DeactivateShieldAfterTime(duration));  // Deactivate shield after duration
    }

    // Deactivate the shield after the given duration
    private IEnumerator DeactivateShieldAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        DeactivateShield();
    }

    // Deactivate the shield and start cooldown
    private void DeactivateShield()
    {
        isShieldActive = false;
        playerStats.RemoveDefense(blockAmount); // Remove the defense when the shield deactivates
        StartCoroutine(ShieldCooldown());
    }

    // Start the shield's cooldown period
    private IEnumerator ShieldCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);  // Wait for cooldown time
        isOnCooldown = false;  // Reset cooldown
    }

    // Method to block projectiles
    public void BlockProjectile()
    {
        if (isShieldActive)
        {
            // Handle blocking the projectile (destroy or prevent damage)
            Debug.Log("Projectile Blocked");
            DeactivateShield();
        }
    }

    // Method to block melee attacks
    public void BlockMelee()
    {
        if (isShieldActive)
        {
            // Handle blocking the melee attack (reduce damage or prevent damage)
            Debug.Log("Melee Blocked");
            DeactivateShield();
        }
    }
}

    /* Pickup logic for when the player interacts with the shield
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponHolder weaponHolder = other.GetComponent<WeaponHolder>();
            if (weaponHolder != null)
            {
                // Equip the shield and activate it
                weaponHolder.EquipShield(gameObject);
                Destroy(gameObject);  // Destroy the shield pickup object
            }
        }
    }
}
    */