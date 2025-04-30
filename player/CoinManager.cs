using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount = 0;
    public Text coinText;
    [SerializeField] AudioClip coinpickup;

    void Start()
    {
        UpdateCoinText();
    }


    public AudioClip returnCoinSound()
    {
        return coinpickup;
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins Collected: " + coinCount.ToString();
        }
        else
        {
            Debug.LogWarning("CoinText reference is missing!");
        }
    }
}


public class Coin : MonoBehaviour
{
        private AudioClip coinAudio;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            Debug.Log("inside on trigger coin");
            CoinManager coinManager = FindObjectOfType<CoinManager>();

            coinAudio = coinManager.returnCoinSound();
            Sound.instance.PlaySoundFXClip(coinAudio, transform, 1f);
            if (coinManager != null)
            {

                coinManager.AddCoin(1);
                Debug.Log("Coin picked up! Total coins: " + coinManager.coinCount);
                Destroy(gameObject); // Remove the coin from the scene
            }
            else
            {
                Debug.LogWarning("CoinManager not found!");
            }
        }
    }
}
