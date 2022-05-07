using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CalendarManager : MonoBehaviour
{
    [SerializeField] private Dial outerDial;
    [SerializeField] private Dial innerDial;
    [SerializeField] private Button showCodeButton;
    [SerializeField] private Button hideCodeButton;
    [SerializeField] private GameObject code;
    [SerializeField] private AudioClip gearUnlockClip;

    private bool gameFinished;

    private void Start () {
        gameFinished = false;

        showCodeButton.onClick.AddListener(ShowCode);
        hideCodeButton.onClick.AddListener(HideCode);
    }

    private void ShowCode () {
        code.SetActive(true);
        hideCodeButton.gameObject.SetActive(true);
    }

    private void HideCode () {
        code.SetActive(false);
        hideCodeButton.gameObject.SetActive(false);
    }

    private void Update () 
    {
        if (outerDial.GetChosenValue() == 5 && innerDial.GetChosenValue() == 12 && !gameFinished)
        {
            AudioSource.PlayClipAtPoint(gearUnlockClip, Vector3.zero);

            Invoke("LoadNextScene", 3f);

            Debug.Log("POBEDIO SI");

            gameFinished = true;
        }
    }

    private void LoadNextScene()
    {
        CustomSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
