using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    [SerializeField] private Dial outerDial;
    [SerializeField] private Dial innerDial;

    private void Update () {
        if (outerDial.GetChosenValue() == 5 && innerDial.GetChosenValue() == 10) {
            Debug.Log("POBEDIO SI");
        }
    }
}
