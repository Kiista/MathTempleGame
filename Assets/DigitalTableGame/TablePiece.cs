using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TablePiece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;
    private CanvasGroup canvasGroup;
    private int myNumber;
    private TableSlot mySlot;


    public RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (mySlot != null)
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
        Debug.Log(myNumber);
    }

    public void SetSlot(TableSlot slot)
    {
        mySlot = slot;
    }

    public void SetNumber(int number)
    {
        myNumber = number;
    }

    public int GetNumber()
    {
        return myNumber;
    }
}
