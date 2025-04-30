using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound instance;
    [SerializeField] private AudioSource soundFXObject;

    private AudioSource runningAudioSource; // dedicated for looping sounds

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign audioClip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;
        //play sound
        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
        /*
        //get length of clip
        float clipLength = audioSource.clip.length;
        //destroy the clip after its done playing
        Destroy(audioSource.gameObject, clipLength);
        */
    }

    public void PlayLoopingSound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        if (runningAudioSource == null)
        {
            // Create a new AudioSource if not already created
            runningAudioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
            runningAudioSource.loop = true; // Ensure it's set to loop
        }

        if (runningAudioSource.clip != audioClip || !runningAudioSource.isPlaying)
        {
            runningAudioSource.clip = audioClip;
            runningAudioSource.volume = volume;
            runningAudioSource.Play();
        }
    }

    public void StopLoopingSound()
    {
        if (runningAudioSource != null && runningAudioSource.isPlaying)
        {
            runningAudioSource.Stop();
        }
    }


    public void PlayPersistentSound(AudioClip clip)
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