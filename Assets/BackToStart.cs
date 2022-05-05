using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStart : MonoBehaviour
{
    private void OnEnable()
    {
        CustomSceneManager.LoadScene(0);
    }
}
