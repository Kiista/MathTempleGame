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

    private void Start()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            pieces[i].SetNumber(numbers[i]);
        }
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
            CustomSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("WORKED");
        }
        else if (sum != 18 && filled)
        {
            Debug.Log("Fuck my life");
        }
    }
}