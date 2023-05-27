using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    static BackgroundMusic instance;

    // Drag in the .mp3 files here, in the editor
    public AudioClip[] MusicClips;

    public AudioSource Audio;

    // Singelton to keep instance alive through all scenes
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Hooks up the 'OnSceneLoaded' method to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Initialize the AudioSource component
        Audio = GetComponent<AudioSource>();
    }

    // Called whenever a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        // Replacement variable (doesn't change the original audio source)
        AudioSource source = Audio;

        // Debug logging
        Debug.Log("Scene Loaded: " + scene.name);
        Debug.Log("MusicClips Length: " + MusicClips.Length);

        // Plays different music in different scenes
        switch (scene.name)
        {
            case "Gameplay Scenes":
                source.clip = MusicClips[0];
                break;
            case "Gameplay Scenes 2":
                source.clip = MusicClips[1];
                break;
            default:
                source.clip = MusicClips[1];
                break;
        }

        // Debug logging
        if (source.clip != null)
        {
            Debug.Log("Source Clip: " + source.clip.name);
        }
        else
        {
            Debug.Log("Source Clip is null");
        }

        // // Only switch the music if it changed
        // if (source.clip != Audio.clip)
        // {
        //     Debug.Log("masuk sini bagian clip");
        //     Audio.enabled = false;
        //     Audio.clip = source.clip;
        //     Audio.enabled = true;
        // }
        Audio.Play();
    }
}