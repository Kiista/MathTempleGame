using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PayingManager : MonoBehaviour
{
    [SerializeField] private MarketSlot[] slots;
    [SerializeField] private MarketPiece[] pieces;
    [SerializeField] private int[] numbers;


    private void Start()
    {
        pieces = pieces.OrderBy(marketPiece => Guid.NewGuid()).ToArray();

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

        foreach (var marketSlot in slots)
        {
            var marketPiece = marketSlot.GetPiece();

            if (marketPiece != null) sum += marketPiece.GetNumber();
            else
            {
                filled = false;
            }
        }

        if (filled)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
