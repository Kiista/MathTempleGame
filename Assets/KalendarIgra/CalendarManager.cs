using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CalendarManager : MonoBehaviour
{
    [SerializeField] private Dial outerDial;
    [SerializeField] private Dial innerDial;

    private void Update () {
        if (outerDial.GetChosenValue() == 5 && innerDial.GetChosenValue() == 10) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("POBEDIO SI");
        }
    }
}
