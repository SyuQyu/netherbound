
using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ResetTheGame()
    {
        SceneManager.LoadScene("Gameplay Scenes");
        Debug.Log("Press Retry Working");
    }
}
