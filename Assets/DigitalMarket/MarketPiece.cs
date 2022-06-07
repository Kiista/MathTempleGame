using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MarketPiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;
    private CanvasGroup canvasGroup;
    private int myNumber;
    private MarketSlot mySlot;
    private RectTransform rectTransform;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        SetNumber(Random.Range(1, 3));
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if(mySlot != null)
        {
            mySlot.Clear();
            mySlot = null;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }
    public void SetSlot(MarketSlot slot)
    {
        mySlot = slot;
    }

    public void SetNumber(int number)
    {
        myNumber = number;

        var index = number - 1;
        if (number >= 5) index--;

        image.sprite = sprites[index];
    }

    public int GetNumber()
    {
        return myNumber;
    }
}
