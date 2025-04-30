using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    public CanvasGroup IntroTextCanvas;
    private float displayTime = 7f;
    private float fadeDuration = 1f;
    private bool isPaused = false;
    [SerializeField] AudioClip acceptClip;

    public static bool GameActive {  get; private set; }
    void Start()
    {
        GameActive = true;
        IntroTextCanvas.gameObject.SetActive(true);
        StartCoroutine(FadeOutText());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IntroTextCanvas.gameObject.SetActive(false);
            
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
               
                PauseGame();
            }
        }

    }

    IEnumerator FadeOutText()
    {

        yield return new WaitForSeconds(displayTime);
        IntroTextCanvas.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        Sound.instance.PlaySoundFXClip(acceptClip, transform, 1f);
        PausePanel.SetActive(false);
        Time.timeScale = 1f;    //resume game 
        isPaused = false;

        GameActive = true;
    }

    public void ReturnToMain()
    {
        Sound.instance.PlayPersistentSound(acceptClip);
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(0);
        PausePanel.SetActive(false);
    }

    void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;    // pause the game
        isPaused = true;

        GameActive = false;

    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // If inside editor stop the play mode
        #else
            Application.Quit();  // If in build then quit
        #endif
    }
}
