using UnityEngine;

public class SimplePauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas; // Reference to the PauseMenuCanvas
    public AudioSource musicSource; // Reference to the music
    public AudioClip recordScratch; // Record scratch sound effect
    public bool isGamePaused = false;
    public static bool IsGamePaused { get; private set; } // This allows other scripts to check if the game is paused

    void Update()
    {
        // Toggle pause menu when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (recordScratch != null)
        {
            AudioSource.PlayClipAtPoint(recordScratch, Vector3.zero);
        }

        pauseMenuCanvas.SetActive(true);

        if (musicSource != null)
        {
            musicSource.Pause();
        }

        Time.timeScale = 0;
        IsGamePaused = true; // Tell other scripts the game is paused
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuCanvas.SetActive(false);

        if (musicSource != null)
        {
            musicSource.UnPause(); //  Resume from where it left off
        }

        Time.timeScale = 1;
        IsGamePaused = false;
        isGamePaused = false;
    }

    public void QuitGame()
    {
        // Resume time before quitting
        Time.timeScale = 1;
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
