using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CustomSceneManager : MonoBehaviour {

    private static CustomSceneManager instance;
    

    public static void LoadScene (int sceneIndex) {
        if (instance != null) {
            instance.LoadSceneAnimated(sceneIndex);
        }
    }

    [SerializeField] private Image backgroundImage;

    private void LoadSceneAnimated (int sceneIndex) {
        StartCoroutine(LoadSceneAnimatedCoroutine(sceneIndex));
    }

    private IEnumerator LoadSceneAnimatedCoroutine (int sceneIndex) {
        float t = 0f;

        while (t < 0.5f) {
            var alpha = Mathf.Lerp(0f, 1f, t * 2f);
            backgroundImage.color = new Color(0f, 0f, 0f, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        backgroundImage.color = new Color(0f, 0f, 0f, 1f);

        SceneManager.LoadScene(sceneIndex);

        t = 0f;
        while (t < 0.5f) {
            var alpha = Mathf.Lerp(1f, 0f, t * 2f);
            backgroundImage.color = new Color(0f, 0f, 0f, alpha);
            t += Time.deltaTime;
            yield return null;
        }

        backgroundImage.color = new Color(0f, 0f, 0f, 0f);
    }

    private void Start () {
        DontDestroyOnLoad(transform.parent);
        instance = this;
    }
}