using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // Panel to toggle on/off
    public Transform itemsParent; // Parent object to hold item UI elements (e.g., "Weapon" and "WeaponText")
    public Image weaponIcon; // Reference to the Image for Weapon icon
    public Text weaponNameText; // Reference to the Text for Weapon name
    public Image bonusHeartIcon; // Reference to the Image for Bonus Heart
    public Text bonusHeartText; // Reference to the Text for Bonus Heart (if any)
    public GameObject exitPanel; // Reference to the ExitPanel
    public Text exitText; // Reference to the ExitText
    public Image spriteIcon; // Reference to the hardcoded sprite image in the InventoryPanel
    public Text OpenInventoryText;
    public Sprite defaultWeaponIcon; // Default weapon icon sprite
    private string defaultWeaponMessage = "Default weapon Attack Damage: 5"; // Default weapon message
    public SpriteRenderer playerSpriteRenderer;
    public Text attempNo;
    public Inventory inventory;
    public PlayerStats playerStats; // Reference to PlayerStats to check flags
    public Text bestCoins;
    public CoinManager coins;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        playerStats = FindObjectOfType<PlayerStats>(); // Get reference to PlayerStats
        inventoryPanel.SetActive(false); // Hide inventory initially
        exitPanel.SetActive(false); // Hide exit panel initially
        spriteIcon.gameObject.SetActive(false); // Hide sprite icon initially
        weaponNameText.gameObject.SetActive(false);
        weaponIcon.gameObject.SetActive(false);
        OpenInventoryText.gameObject.SetActive(true);
        bonusHeartIcon.gameObject.SetActive(true);
        bonusHeartText.gameObject.SetActive(true);
        bonusHeartText.text = "Bonus Hearts Buff: 0/5";
        UpdateAttemptNumber();
        UpdateBestCoins();
    }

    void Update()
    {
        // Toggle the inventory panel when the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateUI();
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        // Toggle visibility of the inventory panel
        bool isInventoryOpen = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isInventoryOpen); // Toggle the inventory visibility

        // Update other UI elements
        spriteIcon.gameObject.SetActive(true);
        //weaponIcon.gameObject.SetActive(true);

        if (isInventoryOpen)
        {
            // If the inventory is open, hide the "Open Inventory" text
            OpenInventoryText.gameObject.SetActive(true); // Show the "Open Inventory" text again
        }
        else
        {
            // If the inventory is closed, hide the "Open Inventory" text
            OpenInventoryText.gameObject.SetActive(false);
            UpdateUI(); // Update the UI when the inventory is toggled
        }
    }

    public void UpdateUI()
    {
        // Clear existing UI elements (if any) to update dynamically
        //ResetUI();

        // Check which weapon is currently equipped and update the UI based on `ItemData`
        ItemData equippedWeapon = null;
        if (playerSpriteRenderer != null)
        {
            // Set the UI image's sprite to match the player's current sprite
            spriteIcon.sprite = playerSpriteRenderer.sprite;
        }

        // Check if the player has specific weapons equipped
        if (playerStats.hasAxe)
        {
            // If the player has an axe equipped, get the axe item data
            equippedWeapon = inventory.GetWeaponItem("Axe");
        }
        else if (playerStats.hasWeapon) // Check if player has any other weapon (like a sword)
        {
            // If the player has a sword or other weapon equipped, get the item data for it
            equippedWeapon = inventory.GetWeaponItem("Sword");
        }

        if (equippedWeapon != null)
        {
            // If a weapon is equipped, update the UI with the weapon's data
            UpdateWeaponUI(equippedWeapon);
        }
        else
        {
            Debug.Log("Default weapon");
            // If no weapon is equipped, show the default weapon
            weaponIcon.sprite = defaultWeaponIcon;
            weaponNameText.text = defaultWeaponMessage;
        }

        // Update bonus heart text based on the current number of bonus hearts
        int currentBonusHearts = FindObjectOfType<Health>().bonusHearts;
        bonusHeartText.text = $"Bonus Hearts Buff: {currentBonusHearts}/5";

        weaponNameText.gameObject.SetActive(true); // Ensure the weapon name text is visible
        weaponIcon.gameObject.SetActive(true); // Ensure the weapon icon is visible
        exitPanel.SetActive(true);
        exitText.text = "Press E to Close"; // Update exit text
    }

    private void UpdateAttemptNumber()
    {
        // Set the attempt number text to show the current run count from SaveInventory
        attempNo.text = "Attempt Number: " + SaveInventory.runCounter.ToString();
    }

    private void UpdateBestCoins()
    {
        // Check and update the coin high score
        int currentCoins = coins.coinCount; // Assuming you have a method to get current coin count from CoinManager
        if (currentCoins > SaveInventory.bestCoins)
        {
            SaveInventory.bestCoins = currentCoins; // Update the high score if the current coins are higher
        }

        // Display the best coin high score
        bestCoins.text = "Coin High Score: " + SaveInventory.bestCoins.ToString();
    }


    // Helper method to update the weapon UI using data from ItemData
    public void UpdateWeaponUI(ItemData weaponData)
    {
        weaponIcon.sprite = weaponData.icon; // Set the weapon icon from the ItemData
        weaponNameText.text = $"{weaponData.itemName} Weapon Attack Damage: {weaponData.attackBuffAmount}"; // Set weapon name and damage
        weaponIcon.preserveAspect = true; // Ensure the aspect ratio is preserved
    }

    // Reset or clear all UI elements before updating with new items
    private void ResetUI()
    {
        weaponIcon.sprite = null; // Clear weapon icon
        weaponNameText.text = ""; // Clear weapon name
    }
}
