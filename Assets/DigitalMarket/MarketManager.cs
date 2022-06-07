using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private MarketSlot[] slots;
    [SerializeField] private MarketPiece[] pieces;
    [SerializeField] private int[] numbers;
    [SerializeField] private Text sumText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] Text loseText;
    //[SerializeField] GameObject endScreenButton;
    //[SerializeField] GameObject playAgainButton;
    [SerializeField] private GameObject payment;
    [SerializeField] private Button showCodeButton;
    [SerializeField] private Button hideCodeButton;
    [SerializeField] private GameObject code;

    private void Start()
    {
        pieces = pieces.OrderBy(marketPiece => Guid.NewGuid()).ToArray();

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

        foreach (var marketSlot in slots)
        {
            var marketPiece = marketSlot.GetPiece();

            if (marketPiece != null) sum += marketPiece.GetNumber();
            else
            {
                filled = false;
            }
        }

        if (sum == 8 && filled)
        {
            payment.SetActive(true);
        }
        //else if (sum != 8 && filled)
        //{
            //winScreen.SetActive(true);
            //endScreenButton.SetActive(false);
        //}
        sumText.text = $"{sum}g";
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