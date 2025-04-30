using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private AudioClip acceptClip;
    public void PlayGame()
    {
        //SceneManager.LoadSceneAsync(1);
        //Sound.instance.PlaySoundFXClip(acceptClip, transform, 1f);
        Time.timeScale = 1f;
        PlayPersistentSoundChange(acceptClip);
        SceneManager.LoadSceneAsync(1);
    }

    public void GoToCredits()
    {
        PlayPersistentSoundChange(acceptClip);
        SceneManager.LoadSceneAsync(2);
    }

    public void EndGame()
    {
        PlayPersistentSoundChange(acceptClip);
        Application.Quit();

    }
    public void GoBackToMainMenu()
    {
        PlayPersistentSoundChange(acceptClip);
        SceneManager.LoadSceneAsync(0);
    }


    


    private void PlayPersistentSoundChange(AudioClip clip)
    {
        // Create a GameObject to hold the persistent AudioSource
        GameObject soundPlayer = new GameObject("PersistentSoundPlayer");
        AudioSource audioSource = soundPlayer.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.Play();

        // Ensure the sound persists across scenes
        DontDestroyOnLoad(soundPlayer);

        // Destroy the sound player after the clip finishes
        Destroy(soundPlayer, clip.length);
    }
}
