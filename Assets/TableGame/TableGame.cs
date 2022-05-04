using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TableGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            CustomSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
