using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBackgroundMusic : MonoBehaviour
{
    private static SceneBackgroundMusic instance;

    [SerializeField]
    private AudioClip[] sceneBackgroundMusic;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += PlaySongForChangedScene;
        PlaySong(sceneBackgroundMusic[SceneManager.GetActiveScene().buildIndex]);

    }

    private void OnValidate()
    {
        if (sceneBackgroundMusic != null)
        {
            Array.Resize(ref sceneBackgroundMusic, SceneManager.sceneCountInBuildSettings);
        }
        else
        {
            sceneBackgroundMusic = new AudioClip[SceneManager.sceneCountInBuildSettings];
        }
    }

    public void PlaySongForChangedScene(Scene oldScene, Scene newScene)
    {
        var newBackgroundClip = sceneBackgroundMusic[newScene.buildIndex];
        if (audioSource.clip != newBackgroundClip && newBackgroundClip != null)
        {
            PlaySong(newBackgroundClip);
        }
    }

    public void PlaySong(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
