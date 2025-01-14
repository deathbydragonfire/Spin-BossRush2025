using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject volumePanel;
    public GameObject controlsPanel;

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void AdjustVolume(float value)

    {
        AudioListener.volume = value; // Adjusts global audio volume
        Debug.Log($"Volume set to: {value}");
    }

    public void OpenVolume()
    {
        settingsPanel.SetActive(false);
        volumePanel.SetActive(true);
    }

    public void OpenControls()
    {
        settingsPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }

    public void BackToSettings()
    { 
        Debug.Log("Back to Settings Triggered!");
        controlsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
