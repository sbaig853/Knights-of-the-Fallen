using System;
using System.Collections;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D[] _colliders;
    public DropTable dropTable; // Reference to the DropTable component

    public int health = 20; // Starting health for the enemy
    private bool hasDied = false;
    [SerializeField] private float deathAnimationDelay = 0.5f;
    [SerializeField] private AudioClip killedEnemySound;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (hasDied) return;
        health -= damage; // Subtract health by damage

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Sound.instance.PlaySoundFXClip(killedEnemySound,transform,1f);
        Debug.Log("Enemy Die");
        if (hasDied) return;
        hasDied = true;
        AnimatedDeath();
        DropItem();
    }

    private void AnimatedDeath()
    {
        _animator.SetTrigger("die");

        foreach (var boxCollider2D in _colliders)
        {
            boxCollider2D.enabled = false;
        }

        StartCoroutine(WaitCoroutine(() =>
        {
            Destroy(gameObject);
        }, deathAnimationDelay));
    }

    private void DropItem()
    {
        if (dropTable != null)
        {
            DropEntry droppedEntry = dropTable.GetRandomDrop(); // Get the DropEntry which contains both ItemData and prefab
            if (droppedEntry != null)
            {
                ItemData droppedItemData = droppedEntry.item; // Access the ItemData from DropEntry
                GameObject itemPrefab = droppedEntry.itemPrefab; // Access the prefab

                float dropHeightOffset = 0.05f;
                GameObject droppedItem = Instantiate(itemPrefab, transform.position + new Vector3(0, dropHeightOffset, 0), Quaternion.identity);

                droppedItem.transform.rotation = Quaternion.Euler(0, 0, 0); // Ensure it stays upright

                Rigidbody2D rb = droppedItem.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = new Vector2(0, 2); // Adjust drop speed
                    rb.bodyType = RigidbodyType2D.Kinematic; // Disable physics interactions after dropping
                }

                Item itemComponent = droppedItem.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.Setup(droppedItemData); // Setup the dropped item with its ItemData
                }
            }
        }
    }

    private IEnumerator WaitCoroutine(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
