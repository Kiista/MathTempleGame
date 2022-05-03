using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private PuzzleSlot[] slots;
    [SerializeField] private PuzzlePiece[] pieces;
    [SerializeField] private int[] numbers;
    [SerializeField] private Text sumText;
    [SerializeField] private GameObject winScreen;

    private void Start()
    {
        pieces = pieces.OrderBy(puzzlePiece => Guid.NewGuid()).ToArray();
        
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

        foreach (var puzzleSlot in slots)
        {
            var puzzlePiece = puzzleSlot.GetPiece();

            if (puzzlePiece != null) sum += puzzlePiece.GetNumber();
        }

        if (sum == 20)
        {
            winScreen.SetActive(true);
            return;
        }
        sumText.text = $"{sum}";
    }
}