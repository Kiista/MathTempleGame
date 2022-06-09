using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TableManager : MonoBehaviour
{
    [SerializeField] private TableSlot[] slots;
    [SerializeField] private TablePiece[] pieces;
    [SerializeField] private int[] numbers;
    [SerializeField] private Button showCodeButton;
    [SerializeField] private Button hideCodeButton;
    [SerializeField] private GameObject code;

    private void Start()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            pieces[i].SetNumber(numbers[i]);
        }
        showCodeButton.onClick.AddListener(ShowCode);
        hideCodeButton.onClick.AddListener(HideCode);
    }

    private void Update()
    {
        ShowSum();
    }

    private void ShowSum()
    {
        var sum = 0;
        bool filled = true;

        foreach (var puzzleSlot in slots)
        {
            var puzzlePiece = puzzleSlot.GetPiece();

            if (puzzlePiece != null) sum += puzzlePiece.GetNumber();
            else
            {
                filled = false;
            }
        }

        if (sum == 18 && filled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (sum != 18 && filled)
        {
            Debug.Log("Fuck my life");
        }
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
}