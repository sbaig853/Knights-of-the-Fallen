using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] AudioClip audioclip;
    public GameObject gameOverUI;
    public Image nextWeaponImage; // Reference to the Image that shows the next weapon icon
    public Text nextWeaponText;   // Reference to the Text that shows the next weapon description
    private ItemData selectedItemData; // Store selected item data
    public playerMovement playerMovement;  // Reference to the PlayerController to call EquipWeapon
    public CoinManager coinCount;
    void Start()
    {
        nextWeaponImage.gameObject.SetActive(false);
        nextWeaponText.gameObject.SetActive(false);
    }

    // Method to set the selected item data
    public void SetSelectedItemData(ItemData itemData)
    {
        selectedItemData = itemData;

        // Update UI elements to display the next weapon's details
        if (selectedItemData != null)
        {
            nextWeaponImage.sprite = selectedItemData.icon;
            nextWeaponText.text = "Play Again With:\n" + selectedItemData.description + "\nAttack Buff: " + selectedItemData.attackBuffAmount;
            nextWeaponImage.gameObject.SetActive(true);  // Show the next weapon icon
            nextWeaponText.gameObject.SetActive(true);   // Show the next weapon description
        }
    }

    // Method to show the game over screen
    public void GameOver()
    {
        if(SaveInventory.bestCoins <= coinCount.coinCount)
        { 
        SaveInventory.bestCoins = coinCount.coinCount;
        }
        SaveInventory.runCounter++;
        gameOverUI.SetActive(true);
    }

    // Play Again method to restart the scene and equip the selected weapon
    public void PlayAgain()
    {
        // Save the selected item data to the persistent data class
        if (selectedItemData != null)
        {
            SaveInventory.selectedItemData = selectedItemData;
            SaveInventory.runCounter++;
        }
        if (SaveInventory.bestCoins <= coinCount.coinCount)
        {
            SaveInventory.bestCoins = coinCount.coinCount;
        }

        // Reload the scene (you can also use the specific scene name or index here)
        SceneManager.LoadSceneAsync(1); // Assuming scene index 1 is the gameplay scene
    }

    // Quit game method
    public void QuitGame()
    {
        if (SaveInventory.bestCoins <= coinCount.coinCount)
        {
            SaveInventory.bestCoins = coinCount.coinCount;
        }
        Sound.instance.PlayPersistentSound(audioclip);
        SaveInventory.runCounter++;
        Application.Quit();
    }
    public void GoBackToMainMenu()
    {
        if (SaveInventory.bestCoins <= coinCount.coinCount)
        {
            SaveInventory.bestCoins = coinCount.coinCount;
        }
        Sound.instance.PlayPersistentSound(audioclip);
        SaveInventory.runCounter++;
        SceneManager.LoadSceneAsync(0);
    }
}
