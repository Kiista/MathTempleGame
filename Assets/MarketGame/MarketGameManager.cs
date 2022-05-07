using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketGameManager : MonoBehaviour
{
    [SerializeField] Text weightCheck;
    [SerializeField] private Button showCodeButton;
    [SerializeField] private Button hideCodeButton;
    [SerializeField] private GameObject code;

    private bool winCondition = false;


    private void Start()
    { 
        showCodeButton.onClick.AddListener(ShowCode);
        hideCodeButton.onClick.AddListener(HideCode);
    }

    private void ShowCode()
    {
        code.SetActive(true);
        hideCodeButton.gameObject.SetActive(true);
    }

    private void HideCode()
    {
        code.SetActive(false);
        hideCodeButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && winCondition == true)
        {
            CustomSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log(weightCheck.text);
            winCondition = false;
        }


        if (Input.GetKeyDown(KeyCode.Q) && winCondition == false)
        {
            weightCheck.text = "100g Avokada, \n ovo je tražena težina, \n sada isplati.";
            winCondition = true;
        }       
    }
}
