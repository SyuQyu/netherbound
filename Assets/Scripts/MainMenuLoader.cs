
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    public String sceneName;
    // Start is called before the first frame update
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
