using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject PausePanel;

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("pause dipencet");
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause(); // Pause the game by setting the time scale to 0
                // You can also add additional code here to display a pause menu or overlay
            }
            else
            {
                Continue(); // Resume the game by setting the time scale back to 1
                // Additional code can be added here to hide the pause menu or overlay
            }
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}