using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathPanelManager : MonoBehaviour
{
    public GameObject keepItemPanel;
    public GameObject keepItemTextPanel;
    public Image[] itemImages;
    public Text[] itemTexts;
    public GameObject[] selectedItems;
    public GameManagerScript gameManager;  // Reference to the GameManagerScript

    private Inventory inventory;
    private List<ItemData> displayedItems = new List<ItemData>();
    private ItemData selectedItemData;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        keepItemPanel.SetActive(false);
        keepItemTextPanel.SetActive(false);
        foreach (GameObject selectedItem in selectedItems)
        {
            selectedItem.SetActive(false);
        }
    }

    public void DisplayItems(List<ItemData> items)
    {
        displayedItems = new List<ItemData>(items);

        for (int i = 0; i < itemImages.Length; i++)
        {
            itemImages[i].gameObject.SetActive(false);
            itemTexts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < Mathf.Min(items.Count, itemImages.Length); i++)
        {
            itemImages[i].sprite = items[i].icon;
            itemTexts[i].text = items[i].description + "\nAttack Buff: " + items[i].attackBuffAmount;
            itemImages[i].gameObject.SetActive(true);
            itemTexts[i].gameObject.SetActive(true);
        }

        keepItemPanel.SetActive(true);
        keepItemTextPanel.SetActive(true);

        int itemCount = Mathf.Min(items.Count, selectedItems.Length);
        for (int i = 0; i < itemCount; i++)
        {
            selectedItems[i].SetActive(true);
        }

        StartCoroutine(SelectRandomItemCycle());
    }

    public void HidePanel()
    {
        keepItemPanel.SetActive(false);
        keepItemTextPanel.SetActive(false);
    }

    private IEnumerator SelectRandomItemCycle()
    {
        int cycleCount = 9;
        int selectedItemIndex = -1;
        List<GameObject> activeItems = new List<GameObject>();

        foreach (GameObject selectedItem in selectedItems)
        {
            if (selectedItem.activeSelf)
            {
                activeItems.Add(selectedItem);
            }
        }

        if (activeItems.Count == 0)
        {
            yield break;
        }

        for (int cycle = 0; cycle < cycleCount; cycle++)
        {
            foreach (GameObject selectedItem in selectedItems)
            {
                selectedItem.SetActive(false);
            }

            selectedItemIndex = Random.Range(0, activeItems.Count);
            activeItems[selectedItemIndex].SetActive(true);

            yield return new WaitForSeconds(0.5f);
        }

        foreach (GameObject selectedItem in selectedItems)
        {
            selectedItem.SetActive(false);
        }

        activeItems[selectedItemIndex].SetActive(true);
        selectedItemData = displayedItems[selectedItemIndex];

        // Disable KeepOnDeath objects before transitioning to the next screen
        DisableKeepOnDeathObjects();

        // Set the selected item data to the GameManager
        gameManager.SetSelectedItemData(selectedItemData);

        // Call gameManager.gameOver() to show the game over screen
        gameManager.GameOver();
    }

    // Method to disable KeepOnDeath objects before transitioning
    private void DisableKeepOnDeathObjects()
    {
        keepItemPanel.SetActive(false);
        keepItemTextPanel.SetActive(false);

        // Disable selected items as well
        foreach (GameObject selectedItem in selectedItems)
        {
            selectedItem.SetActive(false);
        }
    }
}
