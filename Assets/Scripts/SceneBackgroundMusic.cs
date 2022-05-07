using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] sceneBackgroundMusic;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.activeSceneChanged += PlaySongForCurrentScene;
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

    public void PlaySongForCurrentScene(Scene oldScene, Scene newScene)
    {
        var newBackgroundClip = sceneBackgroundMusic[newScene.buildIndex];
        if (audioSource.clip != newBackgroundClip && newBackgroundClip != null)
        {
            audioSource.clip = newBackgroundClip;
            audioSource.Play();
        }
    }
}
