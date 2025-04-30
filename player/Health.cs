using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int health = 3;
    public int maxHealth = 8; // Maximum health limit
    public int bonusHearts = 0; // Variable to track bonus hearts
    public int maxBonusHearts = 2; // Maximum bonus hearts limit

    public Image[] hearts;
    public Image bonusHeartImage; // Reference to the UI Image for first bonus heart
    public Image bonusHeartImage2; // Reference to the UI Image for second bonus heart
    public Image bonusHeartImage3;
    public Image bonusHeartImage4;
    public Image bonusHeartImage5;
    public Sprite fullHeart;
    public Sprite destroyedHeart;

    public RectTransform heartContainer;
    public Vector2 heartStartPosition = new Vector2(10, -10);
    private Animator anim;
    private bool isDead = false;
    public GameManagerScript gameManager;
    [SerializeField] private AudioSource deathSoundEffect;

    // Reference to the DeathPanelManager
    public DeathPanelManager deathPanelManager;

    // Reference to the inventory
    public Inventory inventory;
    [SerializeField] private AudioSource hurtSoundEffect;

    void Start()
    {
        heartContainer.anchoredPosition = heartStartPosition;
        UpdateHeart();
        UpdateBonusHeartsUI();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // First subtract from bonus hearts
        if (bonusHearts > 0)
        {
            int remainingDamage = damage - bonusHearts;
            bonusHearts = Mathf.Max(0, bonusHearts - damage);
            UpdateBonusHeartsUI();

            if (remainingDamage > 0)
            {
                health -= remainingDamage;
            }
        }
        else
        {
            health -= damage;
        }

        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHeart();

        if (health > 0)
        {
            anim.SetTrigger("hurt");
            if (hurtSoundEffect != null) hurtSoundEffect.Play();
        }
        else
        {
            Die();
        }
    }

    private void Die() {
    if (isDead) return;

    isDead = true; 

    if (deathSoundEffect != null) deathSoundEffect.Play();

    anim.SetTrigger("die");
    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    GetComponent<playerMovement>().enabled = false;
        if (inventory.items.Count > 0)
        {
            ShowDeathPanel();
        }
        else
        {
            gameManager.GameOver();
        }
    // Stop other sounds
    if (Sound.instance != null)
    {
        Sound.instance.StopLoopingSound();
    }
        if (isDead) return;

        isDead = true;

        anim.SetTrigger("die");
        Debug.Log("PlayerDying");
        GetComponent<playerMovement>().enabled = false;
        ShowDeathPanel();
       // gameManager.gameOver(); // displays game over screen
        // Stop sound
        if (Sound.instance != null)
        {
            Sound.instance.StopLoopingSound();
        }

    }

    private void ShowDeathPanel()
    {
        // Get the list of items from the inventory
        Debug.Log("Display Panel");
        List<ItemData> items = inventory.items;

        // Display the items in the death panel
        deathPanelManager.DisplayItems(items);
    }

    public void AddHealth(int amount)
    {
        Debug.Log("AddHealth called with amount: " + amount);
        Debug.Log("Current health: " + health);

        if (amount <= 0) return; // Do nothing if amount is zero or negative

        if (health < maxHealth) // If current health is less than max health
        {
            health += amount; // Restore health
            health = Mathf.Clamp(health, 0, maxHealth); // Ensure health doesn't exceed max
            UpdateHeart();
        }
        else // If health is full, add bonus heart
        {
            bonusHearts += amount; // Increase bonus hearts
            bonusHearts = Mathf.Clamp(bonusHearts, 0, maxBonusHearts); // Ensure bonus hearts don't exceed max
            Debug.Log("Bonus hearts increased: " + bonusHearts);
            Debug.Log("UpdateBonusHeartsUI called. Bonus hearts: " + bonusHearts);
            UpdateBonusHeartsUI(); // Update bonus heart UI
        }
    }

    void UpdateHeart()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) // If health is greater than index
            {
                hearts[i].sprite = fullHeart; // Display full heart
            }
            else if (i < maxHealth) // If index is less than maxHealth but greater than health
            {
                hearts[i].sprite = destroyedHeart; // Display broken heart
            }

        }

        UpdateBonusHeartsUI(); // Update bonus heart UI
    }

    void UpdateBonusHeartsUI()
    {
        // Update first through fifth bonus heart UI visibility or state
        bonusHeartImage.gameObject.SetActive(bonusHearts >= 1);
        bonusHeartImage2.gameObject.SetActive(bonusHearts >= 2);
        bonusHeartImage3.gameObject.SetActive(bonusHearts >= 3);
        bonusHeartImage4.gameObject.SetActive(bonusHearts >= 4);
        bonusHeartImage5.gameObject.SetActive(bonusHearts >= 5);
    }


    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
