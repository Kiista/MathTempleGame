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
    [SerializeField] Text loseText;
    [SerializeField] GameObject endScreenButton;
    [SerializeField] GameObject playAgainButton;

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

        if (sum == 20 && filled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (sum != 20 && filled)
        {
            winScreen.SetActive(true);
            endScreenButton.SetActive(false);
            
        }
        sumText.text = $"{sum}";
    }
}